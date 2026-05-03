using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IMessageServices;

namespace Custom_Builds.Core.Services.MessageServices
{
    public class AddMessageService : IAddMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public AddMessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Result<MessageDTO>> Add(SentMessageDTO toAdd)
        {
            // add message to DB
            var result = await _messageRepository.Add(toAdd);
            if (!result.IsSuccess) return result.MapFailure<MessageDTO>();

            return Result<MessageDTO>.Success(result.Value!.toDTO());
        }
    }
}
