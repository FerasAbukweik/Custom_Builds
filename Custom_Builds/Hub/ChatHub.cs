using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts.IChatGroupServices;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using custom_Peripherals.IHub;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace custom_Peripherals.Hub
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IAddMessageService _addMessageService;
        private readonly ICurrTokenService _currUserServices;
        private readonly IGetChatGroupService _getChatGroupService;
                                            //ConnectionId , chatGroupId -- users and admins should be typing in one chat max
        private static readonly ConcurrentDictionary<string, Guid> _usersTyping = new();

        public ChatHub(IAddMessageService addMessageService,
                       ICurrTokenService currUserServices,
                       IGetChatGroupService getChatGroupService)
        {
            _addMessageService = addMessageService;
            _currUserServices = currUserServices;
            _getChatGroupService = getChatGroupService;
        }

        private async Task<Guid?> getUserChatGroupId()
        {
            var getSenderId = _currUserServices.GetUserId();
            if (!getSenderId.IsSuccess) return null;

            var getCurrUserChatGroupIdResult = await _getChatGroupService.GetChatGroupIdAsync(getSenderId.Value!);
            if (!getCurrUserChatGroupIdResult.IsSuccess) return null;

            return getCurrUserChatGroupIdResult.Value;
        }

        private async Task resolveStoppedTyping(Guid chatGroupId)
        {
            List<string> currChatGroupUsersTyping = _usersTyping.Where(ut => ut.Value == chatGroupId).Select(ut => ut.Key).ToList();
            int numberOfUsersTyping = currChatGroupUsersTyping.Count();

            if (numberOfUsersTyping == 0)
            {
                // if no one is typing notify everyone no one is typing
                await Clients.Group(chatGroupId.ToString()).UserStoppedTypingAsync(chatGroupId);
            }
            else if (numberOfUsersTyping == 1)
            {
                // if only one is typing only he should be notified that no one is typing (everyone else should still have userIsTyping)
                await Clients.Client(currChatGroupUsersTyping[0]).UserStoppedTypingAsync(chatGroupId);
            }

            // otherwise everyone in the chat should have someone is typing
        }


        public async Task JoinChatGroup(Guid? chatGroupId = null)
        {
            if (chatGroupId == null) chatGroupId = await getUserChatGroupId();
            if (chatGroupId == null) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId!.Value.ToString());
        }

        public async Task SendMessage(AddMessageDTO toAdd)
        {
            if (toAdd.ChatGroupId == null) toAdd.ChatGroupId = await getUserChatGroupId();
            if (toAdd.ChatGroupId == null) return;

            //store message to DB
            var result = await _addMessageService.AddAsync(toAdd);
            if (!result.IsSuccess) return;

            // send full DTO to receiver
            await Clients.Group(toAdd.ChatGroupId!.Value.ToString()).ReceiveMessageAsync(result.Value!);
        }

        public async Task NotifyTyping(Guid? chatGroupId = null)
        {
            if (chatGroupId == null) chatGroupId = await getUserChatGroupId();
            if (chatGroupId == null) return;

            // if user switched to another chat group, remove previous typing status
            _usersTyping.TryRemove(Context.ConnectionId, out _);

            // add user to users typing dictinary with current chat group id
            _usersTyping.TryAdd(Context.ConnectionId, chatGroupId.Value);

            await Clients.OthersInGroup(chatGroupId.Value.ToString()).UserIsTypingAsync(chatGroupId!.Value);
        }

        public async Task NotifyStoppedTyping(Guid? chatGroupId = null)
        {
            if (chatGroupId == null) chatGroupId = await getUserChatGroupId();
            if (chatGroupId == null) return;

            // remove user from users typing dictinary
            _usersTyping.TryRemove(Context.ConnectionId, out _);

            await resolveStoppedTyping(chatGroupId!.Value);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_usersTyping.TryRemove(Context.ConnectionId, out Guid chatGroupId))
            {
                await resolveStoppedTyping(chatGroupId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}