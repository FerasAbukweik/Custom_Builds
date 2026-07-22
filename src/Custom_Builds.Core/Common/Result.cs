using System.Net;

namespace Custom_Builds.Core.Common
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        protected Result(bool isSuccess, string? errorMessage, HttpStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public static Result Success(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new Result(true, null, statusCode);
        }
        public static Result Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new Result(false, message, statusCode);
        }
        public Result<NewT> MapFailure<NewT>()
        {
            return Result<NewT>.Failure(ErrorMessage ?? "no error message", StatusCode);
        }
    }
    
}