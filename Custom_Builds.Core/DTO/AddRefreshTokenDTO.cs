using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Custom_Builds.Core.DTO
{
    public class AddRefreshTokenDTO
    {
        public required string RefreshTokenString { get; set; }
        public required DateTime ExpierDate { get; set; }
        public required Guid UserID { get; set; }
    }
}
