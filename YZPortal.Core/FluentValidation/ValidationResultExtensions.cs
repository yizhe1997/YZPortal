using FluentValidation.Results;

namespace YZPortal.Core.FluentValidation
{
    public static class ValidationResultExtensions
    {
        public static string ToErrorString(this ValidationResult validationResult)
        {
            return string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage));
        }
    }
}
