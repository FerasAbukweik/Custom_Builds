using Custom_Builds.Core.Domain.Entities;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IChatGroupRepository
    {
        void Add(ChatGroup toAdd);
        Task<ChatGroup?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Guid?> GetUserChatGroupIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
