using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Linq.Expressions;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IModificationsRepository
    {
        Task<Result<Modification>>  GetFromIdAsync(Guid modificationId, Expression<Func<Modification, object>>[]? includes = null);
        Task<Result> EditByIdAsync(EditModificationDTO newData);
        Task<Result<Modification>> AddAsync(Modification toAdd);
        Task<Result> RemoveByIdAsync(Guid modificationId);
        Task<Result<List<Modification>>> FilterAsync(Expression<Func<Modification, bool>> extraChecks);
    }
}
