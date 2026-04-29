using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IGetCartService
    {
        Task<Result<Cart>> GetByIdAsync(Guid cartId);
    }
}
