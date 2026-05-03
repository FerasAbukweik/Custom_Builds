using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositoryContracts
{
    public interface IPartRepository
    {
        Task<Result<Part>> GetByIdAsync(Guid partId);
        Task<Result<Part>> AddAsync(AddPartDTO toAdd);
        Task<Result> RemoveByIdAsync(Guid partId);
        Task<Result> EditByIdAsync(EditPartDTO newData);
    }
}
