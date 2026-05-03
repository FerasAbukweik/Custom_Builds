using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using Custom_Builds.Core.Services.MessageServices;
using custom_Peripherals.IHub;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using System.Security.Claims;

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

        public async Task SendMessage(SentMessageDTO toAdd)
        {
            //store message to DB
            var result = await _addMessageService.Add(toAdd);
            if (!result.IsSuccess) return;


            // send full DTO to receiver
            await Clients.User(toAdd.ReceiverId.ToString()).ReceiveMessageAsync(result.Value!);
            await Clients.Caller.ReceiveMessageAsync(result.Value!);
        }


        public async Task NotifyTyping(Guid receiverId)
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            await Clients.User(receiverId.ToString()).UserIsTypingAsync(getSenderId.Value!);
        }

        public async Task NotifyStoppedTyping(Guid receiverId)
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return;

            await Clients.User(receiverId.ToString()).UserStoppedTypingAsync(getSenderId.Value!);
        }
    }
}