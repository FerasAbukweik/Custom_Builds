using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IRefreshTokenServices
{
    public interface IRemoveRefreshTokenService
    {
        Task<Result> RemoveByRefreshTokenStringAsync(string refreshToken);
    }
}
