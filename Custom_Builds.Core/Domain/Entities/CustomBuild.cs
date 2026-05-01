using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class CustomBuild
    {
        [Key]
        public Guid Id { get; set; }
        public Order? Order { get; set; }
        public CustomBuildTypeEnum CustomBuildType { get; set; } = CustomBuildTypeEnum.CustomPeripheral;

        public List<Modification> Modifications { get; set; } = new List<Modification>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
