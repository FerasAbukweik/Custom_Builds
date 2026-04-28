using Microsoft.AspNetCore.Mvc;
using System.Net;
using Custom_Builds.Core.Models;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => new OkResult(),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(result.ErrorMessage),
            HttpStatusCode.NotFound => new NotFoundObjectResult(result.ErrorMessage),
            HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(result.ErrorMessage),
            _ => new StatusCodeResult((int)result.StatusCode)
        };
    }

    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess && result.StatusCode == HttpStatusCode.OK)
            return new OkObjectResult(result.Value);

        return ((Result)result).ToActionResult();
    }
}