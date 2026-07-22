namespace Custom_Builds.Core.DTO.Part
{
    public class PartEditDTO_DB
    {
        public required Guid Id { get; set; }
        public string? Icon { get; set; }
        public string? Name { get; set; }
        public List<Domain.Entities.Section>? Sections { get; set; }
    }
}
