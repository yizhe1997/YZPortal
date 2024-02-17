using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YZPortal.API.Filters
{
    public class ValidatorActionFilter : ActionFilterAttribute
    {
        private readonly ISerializerService _serializer;

        public ValidatorActionFilter(ISerializerService serializer)
        {
            _serializer = serializer;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if the controller is an API controller
            bool isApiController = filterContext.Controller.GetType().GetCustomAttributes(typeof(ApiControllerAttribute), true).Length > 0;

            if (isApiController)
            {
                // Model validation and error handling
                if (!filterContext.ModelState.IsValid)
                {
                    var result = new ContentResult();
                    var errors = new Dictionary<string, string[]>();

                    foreach (var valuePair in filterContext.ModelState)
                    {
                        errors.Add(valuePair.Key, valuePair.Value.Errors.Select(x => x.ErrorMessage).ToArray());
                    }

                    string content = _serializer.Serialize(new { errors });
                    result.Content = content;
                    result.ContentType = "application/json";

                    filterContext.HttpContext.Response.StatusCode = 422; //unprocessable entity;
                    filterContext.Result = result;
                }

                // Implement Security Headers
                filterContext.HttpContext.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
                filterContext.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                filterContext.HttpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                filterContext.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                filterContext.HttpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
