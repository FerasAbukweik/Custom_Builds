using Custom_Builds.Core.Domain.Entities;

namespace Custom_Builds.Core.DTO
{
    public class EditPartDTO_ToDB
    {
        public required Guid Id { get; set; }
        public string? Icon { get; set; }
        public string? Name { get; set; }
        public List<Section>? Sections { get; set; }
    }
}
