using Custom_Builds.Core.DTO.Section;

namespace Custom_Builds.Core.DTO.Part
{
    public class PartDTO
    {
        public Guid Id { get; set; }
        public required string Icon { get; set; }
        public required string Name { get; set; }
        public required List<SectionDTO> Sections { get; set; }
    }
}
