using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Infrastructure.ExceptionHandlers
{
    internal class ValidationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            if (exception is not ValidationException restException) 
            {
                return false;
            }

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = HttpStatusCode.BadRequest.ToString(),
                Detail = string.Join(" ", restException.Errors),
                Type = restException.GetType().Name
            };

            var innerException = restException.InnerException;
            if (innerException is not null && !string.IsNullOrWhiteSpace(innerException.Message))
                problemDetails.Detail += innerException.Message;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
            });
        }
    }
}
