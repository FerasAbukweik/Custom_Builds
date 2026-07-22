using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Modification
{
    public class ModificationEditDTO
    {
        [Required]
        public required Guid Id { get;set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Icon { get; set; }
        public decimal? Price { get; set; }
    }
}
