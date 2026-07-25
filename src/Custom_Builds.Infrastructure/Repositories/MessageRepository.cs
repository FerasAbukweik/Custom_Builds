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
        public async Task<List<Message>> FilterAsync(
            Expression<Func<Message, bool>> extraChecks,
            Expression<Func<Message, object?>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {

            var query = dbContext.Messages.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(extraChecks).ToListAsync(cancellationToken);
        }
        public void UpdateRange(List<Message> newData)
        {
            dbContext.Messages.UpdateRange(newData);
        }
        public async Task<IReadOnlyList<Message>> LazyGetMessagesAsync(
            LazyDTO lazyLoadData,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await dbContext.Messages
                .AsNoTracking()
                .Where(m => m.ChatGroup!.UserId == userId)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(lazyLoadData.Taken)
                .Take(lazyLoadData.SectionSize)
                .ToListAsync(cancellationToken);
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
