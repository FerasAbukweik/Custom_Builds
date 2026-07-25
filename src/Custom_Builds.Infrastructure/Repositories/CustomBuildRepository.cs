using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.CustomBuild;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class CustomBuildRepository(ApplicationDbContext dbContext) : ICustomBuildRepository
    {
        public void Add(CustomBuild customBuildAdd)
        {
            dbContext.CustomBuilds.Add(customBuildAdd);
        }
        public async Task<CustomBuild?> EditByIdAsync(CustomBuildEditDTO newData, CancellationToken cancellationToken = default)
        {
            CustomBuild? toEdit = await dbContext.CustomBuilds.SingleOrDefaultAsync(c => c.Id == newData.Id, cancellationToken);
            if (toEdit == null) return null;

            toEdit.CustomBuildType = newData.NewCustomBuildType ?? toEdit.CustomBuildType;

            return toEdit;
        }
        public async Task<CustomBuild?> GetByIdAsync(Guid customBuildId,
            Expression<Func<CustomBuild, object?>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.CustomBuilds.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            
            return await query.SingleOrDefaultAsync(c => c.Id == customBuildId, cancellationToken);
        }
        public async Task<CustomBuild?> RemoveByIdAsync(Guid customBuildId, CancellationToken cancellationToken = default)
        {
            CustomBuild? toDel = await GetByIdAsync(customBuildId,[] ,cancellationToken);
            if (toDel == null) return null;

            dbContext.CustomBuilds.Remove(toDel);

            return toDel;
        }
        public async Task<Result<List<CustomBuild>>> FilterAsync(
            Expression<Func<CustomBuild, bool>> extraChecks,
            Expression<Func<CustomBuild, object?>>[]? includes = null)
        {

            var query = dbContext.CustomBuilds.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            List<CustomBuild> customBuilds = await query.Where(extraChecks).ToListAsync();

            return Result<List<CustomBuild>>.Success(customBuilds);
        }
        public async Task<Result<decimal>> GetPriceAsync(Guid customBuildId, CancellationToken cancellationToken = default)
        {
            var priceSum = await dbContext.CustomBuilds
                .AsNoTracking()
                .Where(cb => cb.Id == customBuildId)
                .SelectMany(cb => cb.Modifications)
                .SumAsync(m => m.Price, cancellationToken);

            return Result<decimal>.Success(priceSum);
        }
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

    }
}
