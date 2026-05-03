using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "{0} Is required")]
        public required decimal Price { get; set; }
        public List<string> images { get; set; } = new List<string>() {"No images"};


        public List<Order> Orders { get; set; } = new List<Order>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();


        public ProductDTO toDTO()
        {
            return new ProductDTO()
            {
                Id = this.Id,
                Name = this.Name,
                Price = this.Price,
                images = this.images
            };
        }
    }
}
