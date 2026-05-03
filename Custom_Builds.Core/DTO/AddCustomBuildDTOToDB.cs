using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddCustomBuildDTOToDB
    {
        public required Guid UserId { get; set; }
        public required List<Modification> Modifications { get; set; }
        public CustomBuildTypeEnum CustomBuildType { get; set; }
        public required Guid CreatorId { get; set; }
    }
}
