using Custom_Builds.Core.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.DTO.ChatGroup;

namespace Custom_Builds.Core.Domain.Entities
{
    public class ChatGroup
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        
        // relations
        [Required]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public List<Message> Messages = [];


        // DTO
        // required including User and Messages
        public ChatGroupDTO toDTO()
        {
            return new ChatGroupDTO()
            {
                Id = Id,
                LatestMessageAt = Messages.Max(m => m.CreatedAt),
                UserImageUrl = User?.Image?.ImageUrl ?? "unknown",
                UserName = User?.UserName ?? "unknown"
            };
        }
    }
}