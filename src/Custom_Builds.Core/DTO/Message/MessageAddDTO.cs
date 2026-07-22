using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Message
{
    public class MessageAddDTO
    {
        public required string Content { get; set; }
        public required Guid ChatGroupId { get; set; }
    }
}
