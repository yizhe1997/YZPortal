using Application.Exceptions;
using Application.Interfaces.Services;
using Application.Models;
using Hangfire;
using Hangfire.Dashboard;
using Infrastructure.Services.BackgroundJob;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models.ODataErrors;
using Serilog;
using System.Net;

namespace Infrastructure.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IServiceCollection services) =>
            builder
                .UseMiddlewareExceptionHandler(services)
                .UseSerilogRequestLogging()
                .UseCorsPolicy()
                .UseHsts()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseRequestLocalization(options =>
                {
                    var supportedCultures = new[] { "en", "de" };
                    options.SetDefaultCulture(supportedCultures[0])
                        .AddSupportedCultures(supportedCultures)
                        .AddSupportedUICultures(supportedCultures);
                })
                .UseHangfireDashboard();

        // TODO: take a look at this when moving to .net8 https://www.roundthecode.com/dotnet-tutorials/exception-handling-own-middleware-dotnet-8
        private static IApplicationBuilder UseMiddlewareExceptionHandler(this IApplicationBuilder app, IServiceCollection services)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var methodPathString = context.Request.Method + context.Request.Path;

                    if (contextFeature != null)
                    {
                        var serializer = services.BuildServiceProvider().GetRequiredService<ISerializerService>();
                        var logger = services.BuildServiceProvider().GetRequiredService<ILogger>();
                        var innerException = contextFeature.Error.InnerException;

                        // RestException
                        if (typeof(RestException) == contextFeature.Error.GetType() || typeof(RestException) == innerException?.GetType())
                        {
                            var ex = innerException == null ? (RestException)contextFeature.Error : (RestException)innerException;
                            context.Response.StatusCode = ex.Code == HttpStatusCode.NotFound || ex.Code == HttpStatusCode.NoContent ?
                                context.Response.StatusCode :
                                (int)ex.Code;

                            await context.Response.WriteAsync(serializer?.Serialize(new Result()
                            {
                                Messages = new List<string>() { ex.Error }
                            }) ?? "");

                            logger.Error($"RestException thrown for method {methodPathString}.\r\nStatusCode:\r\n{(int)ex.Code}\r\nError message:\r\n{ex.Error}");
                        }
                        // ODataError
                        else if (typeof(ODataError) == contextFeature.Error.GetType() || typeof(ODataError) == innerException?.GetType())
                        {
                            var ex = innerException == null ? (ODataError)contextFeature.Error : (ODataError)innerException;
                            // TODO: this is not working it always turns to status code 0, try adding use to group again for example...
                            var statusCode = !Enum.TryParse(ex.Error?.Code, out HttpStatusCode httpStatusCode) ? (int)httpStatusCode : context.Response.StatusCode;
                            var errMsg = ex.Error?.Message ?? context.Response.StatusCode.ToString();

                            await context.Response.WriteAsync(serializer?.Serialize(new Result()
                            {
                                Messages = new List<string>() { errMsg }
                            }) ?? "");

                            logger.Error($"ODataError thrown for method {methodPathString}.\r\nStatusCode:\r\n{statusCode}\r\nError message:\r\n{errMsg}");
                        }
                        else
                        {
                            await context.Response.WriteAsync(serializer?.Serialize(new Result()
                            {
                                Messages = new List<string>() { $"Error message - {contextFeature.Error.Message}, inner error message - {(innerException != null ? innerException.Message : "")}" }
                            }) ?? "");

                            logger.Error($"Exception thrown for method {methodPathString}.\r\nStatusCode:\r\n{context.Response.StatusCode}\r\nError message:\r\n{contextFeature.Error.Message}\r\nInner error message:\r\n{(innerException != null ? innerException.Message : "")}");
                        }
                    }
                });
            });

            return app;
        }

        private static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions()
                {
                    Authorization = new IDashboardAuthorizationFilter[]
                    {
                        new HangFireDashboardAuthorizationFilter()
                    },
                    // To remove back link
                    AppPath = null
                })
                .RequireAuthorization("Hangfire");
            });

            return app;
        }

        private static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors("CORS");
    }
}
