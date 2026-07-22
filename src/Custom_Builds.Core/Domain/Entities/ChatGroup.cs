using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
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

        public List<ApplicationUser> Supporters = [];

        public List<Message> Messages = [];


        // DTO
        public ChatGroupDTO toDTO()
        {
            return new ChatGroupDTO()
            {
                Id = Id,
                UserId = UserId
            };
        }
    }
}