using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
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



        public RefreshTokenDTO toDTO()
        {
            return new RefreshTokenDTO()
            {
                Id = this.Id,
                RefreshTokenString = this.RefreshTokenString,
                ExpierDate = this.ExpierDate,
                UserId = this.UserId
            };
        } 
    }
}
