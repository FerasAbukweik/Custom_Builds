using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<Message>> Add(Message newMessage)
        {
            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();

            return Result<Message>.Success(newMessage);
        }
        public async Task<Result<List<Message>>> FilterAsync(Expression<Func<Message, bool>> extraChecks, Expression<Func<Message, object>>[]? includes = null)
        {

            var messageQuery = _dbContext.Messages.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    messageQuery = messageQuery.Include(include);
                }
            }

            List<Message> messages = await messageQuery.Where(extraChecks).ToListAsync();

            return Result<List<Message>>.Success(messages);
        }
        public async Task<Result> UpdateRange(List<Message> newData)
        {
            _dbContext.Messages.UpdateRange(newData);

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
