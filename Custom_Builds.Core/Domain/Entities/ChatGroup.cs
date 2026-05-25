using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class ChatGroup
    {
        [Key]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }



        public List<ApplicationUser> Supporters = new List<ApplicationUser>();

        public List<Message> Messages = new List<Message>();


        public ChatGroupDTO toDTO()
        {
            return new ChatGroupDTO()
            {
                Id = this.Id,
                UserId = this.UserId
            };
        }
    }
}