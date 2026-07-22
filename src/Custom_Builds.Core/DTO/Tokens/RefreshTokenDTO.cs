namespace Custom_Builds.Core.DTO.Tokens
{
    public class RefreshTokenDTO
    {
        public required Guid Id { get; set; }
        public required string RefreshTokenString { get; set; }
        public required DateTime ExpiryDate { get; set; }
        public required Guid UserId { get; set; }
    }
}
