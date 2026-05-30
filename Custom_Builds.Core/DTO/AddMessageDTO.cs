using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddMessageDTO
    {

        [Required(ErrorMessage = "{0} Is required")]
        public required string Content { get; set; }

        [Required(ErrorMessage = "{0} Is reqired")]
        public MessageTypeEnum MessageType { get; set; }
        public string? FileName { get; set; }
    }
}
