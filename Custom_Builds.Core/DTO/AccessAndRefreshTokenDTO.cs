using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.DTO
{
    public class AccessAndRefreshTokenDTO
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
