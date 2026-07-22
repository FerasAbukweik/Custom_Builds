using System.Net;

namespace Custom_Builds.Core.Common;

public class Result<T> : Result
{
    public T? Value { get; set; }

    private Result(T? value, bool isSuccess, string? errorMessage, HttpStatusCode statusCode)
        : base(isSuccess, errorMessage, statusCode)
    {
        Value = value;
    }

    public static Result<T> Success(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new Result<T>(value , true , null , statusCode);
    }
    public new static Result<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>(default , false , message , statusCode);
    }
}