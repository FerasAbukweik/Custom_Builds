using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Tokens;

namespace Custom_Builds.Core.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public required Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string RefreshTokenString { get; set; }

        [Required]
        public required DateTime ExpiryDate { get; set; }
        

        
        // relations
        
        [Required]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }

        
        // methods
        public bool IsExpired => ExpiryDate < DateTime.UtcNow;
        
        
        // DTO
        public RefreshTokenDTO toDTO()
        {
            return new RefreshTokenDTO()
            {
                Id = Id,
                RefreshTokenString = RefreshTokenString,
                ExpiryDate = ExpiryDate,
                UserId = UserId
            };
        } 
    }
}
