using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;


namespace Custom_Builds.Core.extensionMethods
{
    public static class ModelStateExtensionMethods
    {
        public static string CollectErrors(this ModelStateDictionary modelState)
        {
            List<string> AllErrorsList = modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();

            string AllErrorsString = string.Join(" | ", AllErrorsList);

            return AllErrorsString;
        }
    }
}
