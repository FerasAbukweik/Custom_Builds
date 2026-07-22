namespace Custom_Builds.Core.DTO.Modification
{
    public class ModificationDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public required decimal Price { get; set; }
    }
}
