using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditCartItemDTO
    {
        public required Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public OrderTypeEnum? orderType { get; set; }
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
