using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IMessageServices;

namespace Custom_Builds.Core.Services.MessageServices
{
    public class DeleteMessageService : IDeleteMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IGetCurrUserService _getCurrUserService;

        public DeleteMessageService(IMessageRepository messageRepository,
                                 IGetCurrUserService getCurrUserService)
        {
            _messageRepository = messageRepository;
            _getCurrUserService = getCurrUserService;
        }
        public async Task<Result> SetUserMessagesToNull()
        {
            // get curr loggedin user id
            var getCurrUserIdResult = _getCurrUserService.GetUserId();
            if (!getCurrUserIdResult.IsSuccess) return getCurrUserIdResult;

            // get all related user messages
            var getCurrUserMessages = await _messageRepository.FilterAsync(m =>
            (m.SenderId == getCurrUserIdResult.Value! ||
            m.ReceiverId == getCurrUserIdResult.Value!));
            if (!getCurrUserMessages.IsSuccess) return getCurrUserMessages;

            // set curr user id to null in the messages
            for (int i = 0; i < getCurrUserMessages.Value!.Count(); i++)
            {
                if (getCurrUserMessages.Value![i].SenderId == getCurrUserIdResult.Value!)
                {
                    getCurrUserMessages.Value![i].SenderId = null;
                }

                if (getCurrUserMessages.Value![i].ReceiverId == getCurrUserIdResult.Value!)
                {
                    getCurrUserMessages.Value![i].ReceiverId = null;
                }
            }

            var updateResult = await _messageRepository.UpdateRange(getCurrUserMessages.Value!);

            return updateResult;
        }
    }
}
