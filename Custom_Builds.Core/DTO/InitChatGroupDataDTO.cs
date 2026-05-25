namespace Custom_Builds.Core.DTO
{
    public class InitChatGroupDataDTO
    {
        public required Guid UserId { get; set; }
        public required Guid ChatGroupId { get; set; }
    }
}
