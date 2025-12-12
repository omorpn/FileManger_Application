using FileManger_Application.Exception;
using Microsoft.AspNetCore.Identity;


namespace FileManger_Application.Helpers
{
    public static class ResponseResult
    {
        public static Result<T> FromIdentityResult<T>(IdentityResult identityResult, string? successMessage, T? data = default, ErrorType error = ErrorType.Validation)
        {
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(";", identityResult.Errors.Select(e => e.Description));
                return Result<T>.Fail(errors, error);
            }
            return Result<T>.Ok(data!, successMessage ?? "Operation completed successfully");
        }
    }
}
