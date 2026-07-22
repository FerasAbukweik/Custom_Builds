namespace Custom_Builds.Core.DTO.Tokens
{
    public class RefreshTokenAddDTO
    {
        public required string RefreshTokenString { get; set; }
        public required DateTime ExpierDate { get; set; }
        public required Guid UserId { get; set; }
    }
}
