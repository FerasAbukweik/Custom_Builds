using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Linq.Expressions;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface ISectionRepository
    {
        Task<Result<Section>> GetByIdAsync(Guid sectionId);
        Task<Result<Section>> AddAsync(Section toAdd);
        Task<Result> RemoveByIdAsync(Guid sectionId);
        Task<Result> EditByIdAsync(EditSectionDTO newData);
        Task<Result<List<Section>>> FilterAsync(Expression<Func<Section, bool>> extraChecks, Expression<Func<Section, object>>[]? includes = null);
    }
}
