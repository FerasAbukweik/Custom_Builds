using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class ChatGroupRepository(ApplicationDbContext dbContext) : IChatGroupRepository
    {
        public void Add(ChatGroup toAdd)
        {
            dbContext.ChatGroups.Add(toAdd);
        }
        public async Task<ChatGroup?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await dbContext.ChatGroups.AsNoTracking().SingleOrDefaultAsync(cg => cg.UserId == userId, cancellationToken);
        }

        public async Task<Guid?> GetUserChatGroupIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return
                await dbContext.ChatGroups
                    .AsNoTracking()
                    .Where(cg => cg.UserId == userId)
                    .Select(g => g.Id)
                    .SingleOrDefaultAsync(cancellationToken);
        }
        
        public async Task<bool>  SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public IQueryable<ChatGroup> FilterQuery(
            Expression<Func<ChatGroup, object?>>[]? includes = null,
            Expression<Func<ChatGroup, object?>>? orderBy = null,
            bool orderByDescending = false,
            int? skip = null,
            int? take = null)
        {
            var query = dbContext.ChatGroups.AsQueryable().AsNoTracking();

            if (includes != null)
            {
                foreach (var inc in includes)
                {
                    query = query.Include(inc);
                }
            }

            if (orderBy != null)
            {
                if (orderByDescending)
                    query = query.OrderByDescending(orderBy);
                else
                    query = query.OrderBy(orderBy);
            }
            
            if (skip != null) query = query.Skip(skip.Value);
            if (take != null) query = query.Take(take.Value);

            return query;
        }

        public async Task<IReadOnlyList<ChatGroup>> FilterAsync(
            Expression<Func<ChatGroup, bool>> extraChecks,
            Expression<Func<ChatGroup, object?>>[]? includes = null,
            Expression<Func<ChatGroup, object?>>? orderBy = null,
            bool orderByDescending = false,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default)
        {
            return await FilterQuery(includes, orderBy, orderByDescending, skip, take)
                .Where(extraChecks)
                .ToListAsync(cancellationToken);
        }
    }
}
