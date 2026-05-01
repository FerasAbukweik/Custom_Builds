using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Range(typeof(decimal), "0", "100000000", ErrorMessage = "Invalid {0}. {0} should be between {1} and {2}.")]
        public required decimal TotalPrice { get; set; }
        public OrderStateEnum OrderStatus { get; set; } = OrderStateEnum.Pending;

        [Required(ErrorMessage = "{0} is required.")]
        public required OrderTypeEnum OrderType { get; set; }


        public Guid? CustomBuildId { get; set; }
        public CustomBuild? CustomBuild { get; set; }

        [MustHaveOneOnlyAttribute(nameof(CustomBuildId), ErrorMessage = "must have only one of {0} or {1}")]
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
