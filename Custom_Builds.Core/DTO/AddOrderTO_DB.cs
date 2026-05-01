using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddOrderTO_DB
    {
        public required Guid UserId { get; set; }
        public required decimal TotalPrice { get; set; }
        public required OrderTypeEnum OrderType { get; set; }
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
