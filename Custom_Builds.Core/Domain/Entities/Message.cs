using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Message
    {
        [Key]
        public required Guid Id { get; set; }
        
        [Required(ErrorMessage = "{0} Is Required")]
        public required Guid SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }

        [Required(ErrorMessage = "{0} Is Required")]
        public required Guid ReceiverId { get; set; }
        public ApplicationUser? Receiver { get; set; }

        [Required(ErrorMessage = "{0} Is required")]
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "{0} Is reqired")]
        public MessageTypeEnum MessageType { get; set; }

        [Required(ErrorMessage = "{0} Is reqired")]
        public string? FileName { get; set; }

        public MessageDTO toDTO()
        {
            return new MessageDTO()
            {
                Id = this.Id,
                Content = this.Content,
                CreatedAt = this.CreatedAt,
                FileName = this.FileName,
                MessageType = this.MessageType,
                SenderId = this.SenderId,
            };
        }
    }
}