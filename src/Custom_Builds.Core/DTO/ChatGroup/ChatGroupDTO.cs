
namespace Custom_Builds.Core.DTO.ChatGroup
{
    public class ChatGroupDTO
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required DateTime LatestMessageAt { get; set; }
        public required string UserImageUrl { get; set; }
    }
}
