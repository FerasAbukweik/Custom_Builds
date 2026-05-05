using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IMessageServices;

namespace Custom_Builds.Core.Services.MessageServices
{
    public class AddMessageService : IAddMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IGetCurrUserService _getCurrUserService;

        public AddMessageService(IMessageRepository messageRepository,
                                 IGetCurrUserService getCurrUserService)
        {
            _messageRepository = messageRepository;
            _getCurrUserService = getCurrUserService;
        }

        public async Task<Result<MessageDTO>> Add(SentMessageDTO toAdd)
        {
            // get curr logged in user id
            var getCurrUserId = _getCurrUserService.GetUserId();
            if (!getCurrUserId.IsSuccess) return getCurrUserId.MapFailure<MessageDTO>();

            // new message
            Message newMessage = new Message()
            {
                Id = Guid.NewGuid(),
                Content = toAdd.Content,
                CreatedAt = DateTime.UtcNow,
                FileName = toAdd.FileName,
                MessageType = toAdd.MessageType,
                ReceiverId = toAdd.ReceiverId,
                SenderId = getCurrUserId.Value!
            };


            // add message to DB
            var result = await _messageRepository.Add(newMessage);
            if (!result.IsSuccess) return result.MapFailure<MessageDTO>();

            return Result<MessageDTO>.Success(result.Value!.toDTO());
        }
    }
}
