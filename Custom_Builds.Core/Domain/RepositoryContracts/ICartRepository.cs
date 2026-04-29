using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface ICartRepository
    {
        Task<Result<Cart>> GetByIdAsync(Guid cartId);
        Task<Result> AddAsync(AddCartDTO toAdd);
        Task<Result> EditByIdAsync(EditCartDTO newData);
        Task<Result> RemoveByIdAsync(Guid cartId);
    }
}
