using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.Constants
{
    public static class ValidationConstants
    {
        public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
        public const string PhoneNumberRegex = @"^07[789]\d{7}$";
    }
}
