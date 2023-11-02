using Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Extensions
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Fail(result.Errors.Select(e => e.Description).ToList());
        }

        public async static Task<Result> ToApplicationResultAsync(this IdentityResult result)
        {
            return result.Succeeded
                ? await Result.SuccessAsync()
                : await Result.FailAsync(result.Errors.Select(e => e.Description).ToList());
        }
    }
}
