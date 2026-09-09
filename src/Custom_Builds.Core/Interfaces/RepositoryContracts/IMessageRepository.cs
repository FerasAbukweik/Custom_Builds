using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IMessageRepository
    {
        void Add(Message message);

        Task<List<Message>> FilterAsync(
            Expression<Func<Message, bool>> extraChecks,
            Expression<Func<Message, object?>>[]? includes = null,
            Expression<Func<Message, object?>>? orderBy = null,
            bool orderByDescending = false,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);
        void UpdateRange(List<Message> newData);
        
        Task<Message?> GetByIdAsync(Guid messageId,Expression<Func<Message, object?>>[]? include = null, CancellationToken cancellationToken = default);

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
