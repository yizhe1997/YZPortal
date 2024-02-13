using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Attributes
{
    public class ControllerMethodAttribute : ActionFilterAttribute
    {
        public bool SkipHSTSResponseHeader { get; }

        public ControllerMethodAttribute(bool skipHSTSResponseHeader)
        {
            SkipHSTSResponseHeader = skipHSTSResponseHeader;
        }
    }
}
