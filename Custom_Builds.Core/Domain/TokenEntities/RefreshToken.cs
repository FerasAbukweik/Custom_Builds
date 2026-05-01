using Custom_Builds.Core.Domain.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.Domain.TokenEntities
{
    public class RefreshToken
    {
        [Key]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        [Column(TypeName = "varchar")]
        public required string RefreshTokenString { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required DateTime ExpierDate { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required Guid UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
