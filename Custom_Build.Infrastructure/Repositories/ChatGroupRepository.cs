using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class ChatGroupRepository : IChatGroupRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatGroupRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ChatGroup>> AddChatGroupAsync(ChatGroup toAdd)
        {
            _dbContext.ChatGroups.Add(toAdd);

            await _dbContext.SaveChangesAsync();

            return Result<ChatGroup>.Success(toAdd);
        }
        public async Task<Result<Guid>> GetUserChatGroupIdAsync(Guid userId)
        {
            Guid? result = (await _dbContext.ChatGroups.FirstOrDefaultAsync(cg => cg.UserId == userId))?.Id;

            if (result == null)
            {
                return Result<Guid>.Failure("Chat group not found." , HttpStatusCode.NotFound);
            }

            return Result<Guid>.Success(result.Value);
        }
    }
}
