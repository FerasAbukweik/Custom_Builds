namespace Custom_Builds.Core.DTO
{
    public class AddRefreshTokenDTO
    {
        public required string RefreshTokenString { get; set; }
        public required DateTime ExpierDate { get; set; }
        public required Guid UserId { get; set; }
    }
}
