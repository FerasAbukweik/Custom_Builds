using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Custom_Builds.Core.DTO.Message;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        
        // relations

        [Required]
        public required Guid SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }

        [Required]
        public required Guid ChatGroupId { get; set; }
        public ChatGroup? ChatGroup { get; set; }

        
        // DTO
        // must include sender
        public MessageDTO toDTO(Guid currUserId)
        {
            return new MessageDTO()
            {
                Id = Id,
                Content = Content,
                CreatedAt = CreatedAt,
                IsCurrUserSender = SenderId == currUserId,
                SenderName = Sender?.UserName ?? "unknown",
            };
        }
    }
}