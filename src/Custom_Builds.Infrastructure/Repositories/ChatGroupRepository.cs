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
            return await dbContext.ChatGroups.SingleOrDefaultAsync(cg => cg.UserId == userId, cancellationToken);
        }

        public async Task<Guid?> GetUserChatGroupIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return
                await dbContext.ChatGroups
                    .Where(cg => cg.UserId == userId)
                    .Select(g => g.Id)
                    .SingleOrDefaultAsync(cancellationToken);
        }
        
        public async Task<bool>  SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
