using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class CustomBuild
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is required")]
        public CustomBuildTypeEnum CustomBuildType { get; set; }
        public List<Modification> Modifications { get; set; } = new List<Modification>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public List<Order> orders = new List<Order>();
        public required Guid CreatorId { get; set; }
        public ApplicationUser? Creator { get; set; }

        public CustomBuildDTO toDTO()
        {
            return new CustomBuildDTO()
            {
                Id = this.Id,
                CustomBuildType = this.CustomBuildType,
                ModificationsIds = this.Modifications.Select(m => m.Id).ToList()
            };
        } 
    }
}
