using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.DTO.Part;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class PartRepository(ApplicationDbContext dbContext) : IPartRepository
    {
        public void Add(Part toAdd)
        {
            dbContext.Parts.Add(toAdd);
        }
        public async Task<Part?> EditByIdAsync(PartEditDTO newData, CancellationToken cancellationToken = default)
        {
            Part? toEdit = await dbContext.Parts.FindAsync([newData.Id], cancellationToken);

            if (toEdit == null) return null;

            toEdit.Icon = newData.Icon ?? toEdit.Icon;
            toEdit.Name = newData.Name ?? toEdit.Name;

            return toEdit;
        }
        public async Task<Part?> GetByIdAsync(Guid partId, CancellationToken cancellationToken = default)
        {
            var part = await dbContext.Parts.FindAsync([partId], cancellationToken);

            return part;
        }
        public async Task<Part?> RemoveByIdAsync(Guid partId, CancellationToken cancellationToken = default)
        {
            Part? toDel = await dbContext.Parts.FindAsync([partId], cancellationToken);

            if (toDel == null) return null;

            dbContext.Parts.Remove(toDel);

            return toDel;
        }
        public async Task<IReadOnlyList<Part>> FilterAsync(
            Expression<Func<Part, bool>> extraChecks,
            Expression<Func<Part, object>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Parts.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(extraChecks).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Part>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Parts
                .Include(p => p.Sections)
                .ThenInclude(s => s.Modifications)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
