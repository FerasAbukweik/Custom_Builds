using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Custom_Builds.Core.DTO.Product;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string Title { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(500)")]
        public required string Description { get; set; }

        [Required]
        public required decimal Price { get; set; }
        public List<string> Images { get; set; } = ["No images"];

        
        
        // relations

        public List<OrderItem> OrderItems { get; set; } = [];
        public List<CartItem> CartItems { get; set; } = [];
        
        // DTO
        public ProductDTO toDTO()
        {
            return new ProductDTO()
            {
                Id = Id,
                Title = Title,
                Price = Price,
                Images = Images,
                Description = Description
            };
        }
    }
}
