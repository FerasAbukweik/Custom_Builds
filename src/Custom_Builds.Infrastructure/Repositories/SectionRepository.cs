using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.DTO.Section;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class SectionRepository(ApplicationDbContext dbContext) : ISectionRepository
    {
        public void Add(Section toAdd)
        {
            dbContext.Sections.Add(toAdd);
        }
        public async Task<Section?> EditByIdAsync(SectionEditDTO newData, CancellationToken cancellationToken = default)
        {
            Section? toEdit = await dbContext.Sections.SingleOrDefaultAsync(s => s.Id == newData.Id, cancellationToken);

            if (toEdit == null) return null;

            toEdit.Title = newData.Title ?? toEdit.Title;

            return toEdit;
        }
        public async Task<Section?> GetByIdAsync(
            Guid sectionId,
            Expression<Func<Section, object?>>[]? includes,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Sections.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            
            return await query.SingleOrDefaultAsync(s => s.Id == sectionId, cancellationToken);
        }
        public async Task<Section?> RemoveByIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
        {
            Section? toDel = await GetByIdAsync(sectionId,[], cancellationToken);

            if (toDel == null) return null;

            dbContext.Sections.Remove(toDel);

            return toDel;
        }
        public async Task<IReadOnlyList<Section>> FilterAsync(
            Expression<Func<Section, bool>> extraChecks,
            Expression<Func<Section, object?>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Sections.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(extraChecks).ToListAsync(cancellationToken);
        } 
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        { 
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
