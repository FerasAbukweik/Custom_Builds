using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IModificationsRepository
    {
        Task<Result<Modification>> GetFromIdAsync(Guid modificationId);
        Task<Result<List<Modification>>> GetListFromIdsAsync(List<Guid> modificationIds);
        Task<Result> RemoveByIdAsync(Guid modificationId);
        Task<Result> EditByIdAsync(EditModificationDTO newData);
        Task<Result<Modification>> AddAsync(AddModificationDTO toAdd);
    }
}
