using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositoryContracts
{
    public interface IMessageRepository
    {
        Task<Result<Message>> Add(SentMessageDTO message);
    }
}
