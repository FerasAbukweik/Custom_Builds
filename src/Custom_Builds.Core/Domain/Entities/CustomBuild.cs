using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.DTO.CustomBuild;

namespace Custom_Builds.Core.Domain.Entities
{
    public class CustomBuild
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public CustomBuildTypeEnum CustomBuildType { get; set; }
        
        
        // relations
        
        [Required]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        
        public OrderItem? OrderItem { get; set; }
        
        public List<Modification> Modifications { get; set; } = [];
        public List<CartItem> CartItems { get; set; } = [];
        
        
        // DTO
        // must include modifications
        public CustomBuildDTO toDTO()
        {
            return new CustomBuildDTO()
            {
                Id = Id,
                CustomBuildType = CustomBuildType,
                ModificationsIds = Modifications.Select(m => m.Id).ToList()
            };
        } 
    }
}
