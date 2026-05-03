using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public required decimal TotalPrice { get; set; }
        public required OrderTypeEnum orderType { get; set; }
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }

    }
}
