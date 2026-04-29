namespace Custom_Builds.Core.ServiceContracts
{
    public interface IRevokeRefreshTokenService
    {
        Task RemoveByRefreshTokenStringAsync(string refreshToken);
    }
}
