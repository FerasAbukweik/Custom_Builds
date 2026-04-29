using Custom_Builds.Core.Domain.Identity;
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
        [Range(typeof(decimal), "0", "100000000", ErrorMessage = "Invalid {0}. TotalPrice should be between {1} and {2}.")]
        public required decimal TotalPrice { get; set; }
    }
}
