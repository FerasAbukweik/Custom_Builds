using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO
{
    public class MessageDTO
    {
        public required Guid Id { get; set; }
        public required Guid SenderId { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public MessageTypeEnum MessageType { get; set; }
        public string? FileName { get; set; }
    }
}
