using System.Net;
using Custom_Builds.Core.Common;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.ExtensionMethods;

public static class ResultExtensions
{
    public static ActionResult ToActionResult (this Result result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => new OkResult(),
            _ => new ObjectResult(result.ErrorMessage) { StatusCode = (int)result.StatusCode }
        };
    }

    public static ActionResult<T> ToActionResult<T> (this Result<T> result)
    {
        if (result.IsSuccess && result.StatusCode == HttpStatusCode.OK)
            return new OkObjectResult(result.Value);

        return result.StatusCode switch 
        {
            HttpStatusCode.OK => new OkObjectResult(result.Value),
            _ => ToActionResult((Result)result)
        };
    }
}