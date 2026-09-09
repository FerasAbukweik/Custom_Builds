using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class MessageRepository(ApplicationDbContext dbContext) : IMessageRepository
    {
        public void Add(Message newMessage)
        {
            dbContext.Messages.Add(newMessage);
        }
        public IQueryable<Message> FilterQuery(
            Expression<Func<Message, bool>> extraChecks,
            Expression<Func<Message, object?>>[]? includes = null,
            Expression<Func<Message, object?>>? orderBy = null,
            bool orderByDescending = false,
            int? skip = null,
            int? take = null)
        {

            var query = dbContext.Messages.AsQueryable().AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            query = query.Where(extraChecks);
            
            if (orderBy != null)
            {
                if (orderByDescending) query = query.OrderByDescending(orderBy);
                else query = query.OrderBy(orderBy);
            }
            
            if(skip != null) query = query.Skip(skip.Value);
            if(take != null) query = query.Take(take.Value);
            
            return query;
        }

        public async Task<List<Message>> FilterAsync(
            Expression<Func<Message, bool>> extraChecks,
            Expression<Func<Message, object?>>[]? includes = null,
            Expression<Func<Message, object?>>? orderBy = null,
            bool orderByDescending = false,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default)
        {
            return await FilterQuery(extraChecks, includes, orderBy, orderByDescending, skip, take)
                .ToListAsync(cancellationToken);
        }
        public void UpdateRange(List<Message> newData)
        {
            dbContext.Messages.UpdateRange(newData);
        }

        public async Task<Message?> GetByIdAsync(Guid messageId, Expression<Func<Message, object?>>[]? include = null, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Messages.AsNoTracking().AsQueryable();

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return await query.SingleOrDefaultAsync(m => m.Id == messageId ,cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
