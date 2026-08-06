namespace Custom_Builds.Core.DTO.Product
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required string Image { get; set; }
        public required int Stock { get; set; }
    }
}
