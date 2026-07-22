using Custom_Builds.Core.DTO.Modification;

namespace Custom_Builds.Core.DTO.Section
{
    public class SectionDTO
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required List<ModificationDTO> Modifications { get; set; }
    }
}
