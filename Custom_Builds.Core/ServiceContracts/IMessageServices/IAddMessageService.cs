using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IMessageServices
{
    public interface IAddMessageService
    {
        Task<Result<MessageDTO>> Add(SentMessageDTO toAdd);
    }
}
