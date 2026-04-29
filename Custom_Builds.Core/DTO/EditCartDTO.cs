using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditCartDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
