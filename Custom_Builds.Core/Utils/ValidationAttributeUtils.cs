using System;

namespace Custom_Builds.Core.Utils
{
    public static class ValidationAttributeUtils
    {
        public static string GetErrorMessage(string errorMessage, params string[] param)
        {
            return string.Format(errorMessage, param);
        }
    }
}
