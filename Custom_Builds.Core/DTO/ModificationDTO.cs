using Custom_Builds.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.DTO
{
    public class ModificationDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public required string Type { get; set; }
        public string? Icon { get; set; }
        public required decimal Price { get; set; }
    }
}
