using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts.IChatGroupServices;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using custom_Peripherals.IHub;
using Microsoft.AspNetCore.SignalR;

namespace custom_Peripherals.Hub
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IAddMessageService _addMessageService;
        private readonly IGetCurrUserService _currUserServices;
        private readonly IGetChatGroupService _getChatGroupService;

        public ChatHub(IAddMessageService addMessageService,
                       IGetCurrUserService currUserServices,
                       IGetChatGroupService getChatGroupService)
        {
            _addMessageService = addMessageService;
            _currUserServices = currUserServices;
            _getChatGroupService = getChatGroupService;
        }


        public async Task JoinChatGroup()
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            var getCurrUserChatGroupIdResult = await _getChatGroupService.GetChatGroupIdAsync(getSenderId.Value!);
            if (!getCurrUserChatGroupIdResult.IsSuccess) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, getCurrUserChatGroupIdResult.Value!.ToString());
        }
        public async Task SendMessage(AddMessageDTO toAdd)
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            var getCurrUserChatGroupIdResult = await _getChatGroupService.GetChatGroupIdAsync(getSenderId.Value!);
            if (!getCurrUserChatGroupIdResult.IsSuccess) return;

            //store message to DB
            var result = await _addMessageService.AddAsync(toAdd, getCurrUserChatGroupIdResult.Value!);
            if (!result.IsSuccess) return;


            // send full DTO to receiver
            await Clients.Group(getCurrUserChatGroupIdResult.Value!.ToString()).ReceiveMessageAsync(result.Value!);
        }


        public async Task NotifyTyping()
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            var getCurrUserChatGroupIdResult = await _getChatGroupService.GetChatGroupIdAsync(getSenderId.Value!);
            if (!getCurrUserChatGroupIdResult.IsSuccess) return;

            await Clients.Group(getCurrUserChatGroupIdResult.Value!.ToString()).UserIsTypingAsync();
        }

        public async Task NotifyStoppedTyping()
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            var getCurrUserChatGroupIdResult = await _getChatGroupService.GetChatGroupIdAsync(getSenderId.Value!);
            if (!getCurrUserChatGroupIdResult.IsSuccess) return;

            await Clients.Group(getCurrUserChatGroupIdResult.Value!.ToString()).UserStoppedTypingAsync();
        }
    }
}