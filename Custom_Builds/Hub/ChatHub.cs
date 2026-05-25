using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using custom_Peripherals.IHub;
using Microsoft.AspNetCore.SignalR;

namespace custom_Peripherals.Hub
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IAddMessageService _addMessageService;
        private readonly IGetCurrUserService _currUserServices;

        public ChatHub(IAddMessageService addMessageService,
                       IGetCurrUserService currUserServices)
        {
            _addMessageService = addMessageService;
            _currUserServices = currUserServices;
        }


        public async Task JoinChatGroup(string chatGroupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId);
        }
        public async Task SendMessage(AddMessageDTO toAdd)
        {
            //store message to DB
            var result = await _addMessageService.AddAsync(toAdd);
            if (!result.IsSuccess) return;


            // send full DTO to receiver
            await Clients.Group(toAdd.ChatGroupId.ToString()).ReceiveMessageAsync(result.Value!);
        }


        public async Task NotifyTyping(Guid groupId)
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            await Clients.Group(groupId.ToString()).UserIsTypingAsync(getSenderId.Value!);
        }

        public async Task NotifyStoppedTyping(Guid groupId)
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            await Clients.Group(groupId.ToString()).UserStoppedTypingAsync(getSenderId.Value!);
        }
    }
}