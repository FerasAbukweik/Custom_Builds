using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Custom_Builds.Core.DTO
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public required OrderTypeEnum orderType { get; set; }
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }
        public required Decimal TotalPrice { get; set; }
        public required int Quantity { get; set; }
        public required string Title { get; set; }
        public List<string> Specs { get; set; } = new List<string>();
        public required string image { get; set; }
    }
}