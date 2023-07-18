using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models.ODataErrors;
using System.Net;

namespace YZPortal.Core.Error
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        ///     This middleware configures the execption handler to provide clean status and meaningful error msg 
        ///     for frontend UI as well as logging
        /// </summary>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var methodPathString = context.Request.Method + context.Request.Path;

                    if (contextFeature != null)
                    {
                        var serviceProvider = new ServiceCollection()
                          .AddLogging(cfg => cfg.AddConsole())
                          .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Information)
                          .BuildServiceProvider();

                        var logger = serviceProvider.GetService<ILogger<Exception>>();

                        var innerException = contextFeature.Error.InnerException;

                        // RestException
                        if (typeof(RestException) == contextFeature.Error.GetType() || typeof(RestException) == innerException?.GetType())
                        {
                            var ex = innerException == null ? (RestException)contextFeature.Error : (RestException)innerException;
                            context.Response.StatusCode = ex.Code == HttpStatusCode.NotFound || ex.Code == HttpStatusCode.NoContent ?
                                context.Response.StatusCode :
                                (int)ex.Code;

                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = ex.Error
                            }.ToString());

                            logger.LogError($"RestException thrown for method {methodPathString}.\r\nStatusCode:\r\n{(int)ex.Code}\r\nError message:\r\n{ex.Error}");
                        }
                        // ODataError
                        else if (typeof(ODataError) == contextFeature.Error.GetType() || typeof(ODataError) == innerException?.GetType())
                        {
                            var ex = innerException == null ? (ODataError)contextFeature.Error : (ODataError)innerException;
                            // TODO: this is not working it always turns to status code 0, try adding use to group again for example...
                            var statusCode = !Enum.TryParse(ex.Error?.Code, out HttpStatusCode httpStatusCode) ? (int)httpStatusCode : context.Response.StatusCode;
                            var errMsg = ex.Error?.Message ?? context.Response.StatusCode.ToString();

                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = statusCode,
                                Message = errMsg
                            }.ToString());

                            logger.LogError($"ODataError thrown for method {methodPathString}.\r\nStatusCode:\r\n{statusCode}\r\nError message:\r\n{errMsg}");
                        }
                        else
                        {
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = $"Error message - {contextFeature.Error.Message}, inner error message - {(innerException != null ? innerException.Message : "")}"
                            }.ToString());

                            logger.LogError($"Exception thrown for method {methodPathString}.\r\nStatusCode:\r\n{context.Response.StatusCode}\r\nError message:\r\n{contextFeature.Error.Message}\r\nInner error message:\r\n{(innerException != null ? innerException.Message : "")}");
                        }
                    }
                });
            });
        }
    }
}
