using System.Net;

namespace Custom_Builds.Core.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public static Result Success(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new Result { IsSuccess = true, StatusCode = statusCode };
        }
        public static Result Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new Result { IsSuccess = false, ErrorMessage = message, StatusCode = statusCode };
        }
        public Result MapFailure()
        {
            return Result.Failure(ErrorMessage ?? "no error message", StatusCode);
        }
    }
    public class Result<T> : Result
    {
        public T? Value { get; set; }
        public static Result<T> Success(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new Result<T> { IsSuccess = true, Value = value, StatusCode = statusCode };
        }
        public new static Result<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new Result<T> { IsSuccess = false, ErrorMessage = message, StatusCode = statusCode };
        }

        public Result<NewT> MapFailure<NewT>()
        {
            return Result<NewT>.Failure(ErrorMessage ?? "no error message", StatusCode);
        }
    }
}