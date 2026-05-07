using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IMessageServices;

namespace Custom_Builds.Core.Services.MessageServices
{
    public class EditMessageService : IEditMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public EditMessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }


        public async Task<Result> SetUserMessagesToNull(Guid userId)
        {
            // get all related user messages
            var getCurrUserMessages = await _messageRepository.FilterAsync(m =>
            (m.SenderId == userId ||
            m.ReceiverId == userId));
            if (!getCurrUserMessages.IsSuccess) return getCurrUserMessages;

            // set curr user id to null in the messages
            for (int i = 0; i < getCurrUserMessages.Value!.Count(); i++)
            {
                if (getCurrUserMessages.Value![i].SenderId == userId)
                {
                    getCurrUserMessages.Value![i].SenderId = null;
                }

                if (getCurrUserMessages.Value![i].ReceiverId == userId)
                {
                    getCurrUserMessages.Value![i].ReceiverId = null;
                }
            }

            var updateResult = await _messageRepository.UpdateRange(getCurrUserMessages.Value!);

            return updateResult;
        }
    }
}
