using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class CustomBuildDTO
    {
        public Guid Id { get; set; }

        public CustomBuildTypeEnum CustomBuildType { get; set; }

        public required List<Guid> ModificationsIds;
    }
}
