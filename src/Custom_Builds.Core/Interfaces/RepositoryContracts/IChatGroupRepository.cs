using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IChatGroupRepository
    {
        void Add(ChatGroup toAdd);
        Task<ChatGroup?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Guid?> GetUserChatGroupIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ChatGroup>> FilterAsync(
            Expression<Func<ChatGroup, bool>> extraChecks,
            Expression<Func<ChatGroup, object?>>[]? includes = null,
            Expression<Func<ChatGroup, object?>>? orderBy = null,
            bool orderByDescending = false,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);
    }
}
