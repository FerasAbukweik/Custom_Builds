using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class ChatGroupDTO
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
    }
}
