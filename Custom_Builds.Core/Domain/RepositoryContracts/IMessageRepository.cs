using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Linq.Expressions;

namespace Custom_Builds.Core.Domain.RepositoryContracts
{
    public interface IMessageRepository
    {
        Task<Result<Message>> Add(Message message);
        Task<Result<List<Message>>> FilterAsync(Expression<Func<Message, bool>> extraChecks, Expression<Func<Message, object>>[]? includes = null);
        Task<Result> UpdateRange(List<Message> newData);
    }
}
