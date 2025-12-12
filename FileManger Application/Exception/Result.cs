using System.Text.Json.Serialization;

namespace FileManger_Application.Exception
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
        public ErrorType ErrorType { get; set; }
        public Result() { }
        public Result(bool success, T? data, string message, ErrorType errorType) { Success = success; Message = message; Data = data; ErrorType = errorType; }

        public static Result<T> Ok(T data, string? message = null)
        {
            return new Result<T>() { Data = data, Success = true, Message = message ?? "Operation completed successfully" };
        }
        public static Result<T> Fail(string error, ErrorType errorType)
        {
            return new Result<T>(success: false, data: default, message: error, errorType: errorType);
        }
    }
}
