using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Product
{
    public class ProductAddDTO_DB
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required List<string> Images { get; set; }
    }
}
