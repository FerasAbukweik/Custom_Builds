namespace Custom_Builds.Core.DTO.Message
{
    public class MessageDTO
    {
        public required Guid Id { get; set; }
        public required Guid SenderId { get; set; }
        public required string SenderName { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required Guid ChatGroupId { get; set; }
    }
}
