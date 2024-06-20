using Application.Interfaces.Services;
using Application.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph.Models.ODataErrors;
using Serilog;
using System.Net;

namespace Infrastructure.ExceptionHandlers
{
    internal class ODataErrorExceptionHandler(ISerializerService serializer, ILogger logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                var innerException = contextFeature.Error.InnerException;

                if (typeof(ODataError) == contextFeature.Error.GetType() || typeof(ODataError) == innerException?.GetType())
                {
                    var oDataError = innerException == null ? (ODataError)contextFeature.Error : (ODataError)innerException;
                    var errorMsgs = new List<string>();

                    // TODO: check if valid code
                    // TODO: this is not working it always turns to status code 0, try adding use to group again for example...
                    httpContext.Response.StatusCode = !Enum.TryParse(oDataError.Error?.Code, out HttpStatusCode httpStatusCode) ? (int)httpStatusCode : (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    if (!string.IsNullOrWhiteSpace(contextFeature.Error.Message))
                        errorMsgs.Add(contextFeature.Error.Message);

                    if (innerException is not null && !string.IsNullOrWhiteSpace(innerException.Message))
                        errorMsgs.Add(innerException.Message);

                    if (errorMsgs.Count == 0)
                        errorMsgs.Add(httpContext.Response.StatusCode.ToString());

                    await httpContext.Response.WriteAsync(serializer?.Serialize(await Result.FailAsync(errorMsgs)) ?? "", cancellationToken: cancellationToken);

                    // TODO: use instrumentation to store and save logs
                    logger.Error(@$"
                    ODataError thrown for method {httpContext.Request.Method + httpContext.Request.Path}
                    StatusCode:
                    {httpContext.Response.StatusCode}
                    Error message:
                    {contextFeature.Error.Message}
                    Inner error message:
                    {(contextFeature.Error.InnerException?.Message)}");

                    return true;
                }
            }

            return false;
        }
    }
}
