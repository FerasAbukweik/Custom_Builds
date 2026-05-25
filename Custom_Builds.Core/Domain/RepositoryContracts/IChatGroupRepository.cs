using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositoryContracts
{
    public interface IChatGroupRepository
    {
        Task<Result<Guid>> GetUserChatGroupIdAsync(Guid userId);
        Task<Result<ChatGroup>> AddChatGroupAsync(ChatGroup toAdd);
    }
}
