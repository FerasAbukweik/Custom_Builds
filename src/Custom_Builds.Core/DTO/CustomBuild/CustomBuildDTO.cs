using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.CustomBuild
{
    public class CustomBuildDTO
    {
        public Guid Id { get; set; }

        public CustomBuildTypeEnum CustomBuildType { get; set; }

        public required List<Guid> ModificationsIds;
    }
}
