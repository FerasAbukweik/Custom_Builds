using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Linq.Expressions;

namespace Custom_Builds.Core.Domain.RepositoryContracts
{
    public interface IPartRepository
    {
        Task<Result<Part>> GetByIdAsync(Guid partId);
        Task<Result<Part>> AddAsync(Part toAdd);
        Task<Result> RemoveByIdAsync(Guid partId);
        Task<Result> EditByIdAsync(EditPartDTO newData);
        Task<Result<List<Part>>> FilterAsync(Expression<Func<Part, bool>> extraChecks, Expression<Func<Part, object>>[]? includes = null);
        Task<Result<List<Part>>> GetAllPartsIncludingAllData();
        Task<Result> LinkSectionAsync(Guid partId , Section section);
    }
}
