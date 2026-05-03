using Custom_Builds.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public List<string> images { get; set; } = new List<string>() { "No images" };
    }
}
