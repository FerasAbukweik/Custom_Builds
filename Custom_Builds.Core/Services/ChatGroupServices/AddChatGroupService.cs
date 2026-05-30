using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IChatGroupServices;
using Custom_Builds.Core.ServiceContracts.IMessageServices;

namespace Custom_Builds.Core.Services.ChatGroupServices
{
    public class AddChatGroupService : IAddChatGroupService
    {
        private readonly IChatGroupRepository _chatGroupRepository;
        private readonly IAddMessageService _addMessageService;

        public AddChatGroupService(IChatGroupRepository chatGroupRepository,
                                   IAddMessageService addMessageService)
        {
            _chatGroupRepository = chatGroupRepository;
            _addMessageService = addMessageService;
        }


        public async Task<Result<ChatGroupDTO>> AddChatGroupAsync(Guid userId)
        {
            ChatGroup toAdd = new ChatGroup()
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };

            // add to DB
            var AddResult = await _chatGroupRepository.AddChatGroupAsync(toAdd);
            if(!AddResult.IsSuccess) return AddResult.MapFailure<ChatGroupDTO>();

            // add Welcome message
            var messageResult = await _addMessageService.AddAsync(new AddMessageDTO()
            {
                Content = "Welcome to your new chat group! Feel free to ask any questions or share your thoughts here. We're here to help you with anything you need. Enjoy your chat experience!",
                MessageType = MessageTypeEnum.Text,
            }, toAdd.Id);

            return Result<ChatGroupDTO>.Success(AddResult.Value!.toDTO());
        }
    }
}
