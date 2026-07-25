using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.DTO.Modification;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class ModificationsRepository(ApplicationDbContext dbContext) : IModificationsRepository
    {
        public async Task<Modification?> GetByIdAsync(
            Guid modificationId,
            Expression<Func<Modification, object?>>[]? includes,
            CancellationToken  cancellationToken = default)
        {
            var query = dbContext.Modifications.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(m => m.Id == modificationId, cancellationToken);
        }
        public void Add(Modification toAdd)
        {
            dbContext.Modifications.Add(toAdd);
        }
        public async Task<Modification?> EditByIdAsync(ModificationEditDTO newData, CancellationToken cancellationToken = default)
        {
            Modification? toEdit = await dbContext.Modifications.FirstOrDefaultAsync(m => m.Id == newData.Id, cancellationToken);
            
            if (toEdit == null) return null;

            toEdit.Price = newData.Price ?? toEdit.Price;
            toEdit.Icon = newData.Icon ?? toEdit.Icon;
            toEdit.Description = newData.Description ?? toEdit.Description;
            toEdit.Name = newData.Name ?? toEdit.Name;
            toEdit.Value = newData.Value ?? toEdit.Value;

            return toEdit;
        }
        public async Task<Modification?> RemoveByIdAsync(Guid modificationId, CancellationToken cancellationToken = default)
        {
            Modification? toDel = await GetByIdAsync(modificationId,[] ,cancellationToken);

            if (toDel == null) return null;

            dbContext.Modifications.Remove(toDel);

            return toDel;
        }
        public async Task<IReadOnlyList<Modification>> FilterAsync(Expression<Func<Modification, bool>> extraChecks,
            Expression<Func<Modification,  object?>>[]? includes,
            CancellationToken cancellationToken = default)
        {

            var query = dbContext.Modifications.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }

            return await query.Where(extraChecks).ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<Modification, bool>> extraChecks, CancellationToken cancellationToken = default)
        {
            return await dbContext.Modifications.AsNoTracking().CountAsync(extraChecks, cancellationToken);
        }

        public async Task<decimal> GetModificationsPriceAsync(IReadOnlyList<Guid> modificationIds, CancellationToken cancellationToken = default)
        {
            return await dbContext.Modifications.AsNoTracking().Where(m => modificationIds.Contains(m.Id)).SumAsync(m => m.Price, cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
         {
             return await dbContext.SaveChangesAsync(cancellationToken) > 0;
         }
    }
}