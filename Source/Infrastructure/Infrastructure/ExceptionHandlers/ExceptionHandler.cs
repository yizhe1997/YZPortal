using Application.Interfaces.Services;
using Application.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;

namespace Infrastructure.ExceptionHandlers
{
    internal class ExceptionHandler(ISerializerService serializer, ILogger logger) : IExceptionHandler
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
                var errorMsgs = new List<string>();

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
                Exception thrown for method {httpContext.Request.Method + httpContext.Request.Path}
                StatusCode:
                {httpContext.Response.StatusCode}
                Error message:
                {contextFeature.Error.Message}
                Inner error message:
                {(contextFeature.Error.InnerException?.Message)}");

                return true;
            }

            return false;
        }
    }
}
