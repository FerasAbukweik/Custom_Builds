using Custom_Builds.Core.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.DTO
{
    public class RefreshTokenDTO
    {
        public required Guid Id { get; set; }
        public required string RefreshTokenString { get; set; }
        public required DateTime ExpierDate { get; set; }
        public required Guid UserId { get; set; }
    }
}
