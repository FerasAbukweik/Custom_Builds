using custom_Peripherals.IHub;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Custom_Builds.Core.DTO.Message;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;

namespace custom_Peripherals.Hub
{
    public class ChatHub(
        IMessageService messageService,
        IChatGroupService chatGroupService,
        IHttpContextAccessor httpContextAccessor
        ) : Hub<IChatHub>
    {
        private static readonly ConcurrentDictionary<string, Guid> _usersTyping = new();

        // on connected
        public override async Task OnConnectedAsync()
        {
            var chatGroupId = await GetChatGroupIdAsync();
            if (chatGroupId == null) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId.Value.ToString());
        }

        public async Task SendMessage(string content)
        {
            var chatGroupId = await GetChatGroupIdAsync();
            var currUserId = GetUserId();
            
            if (chatGroupId == null || currUserId == null) return;
            
            //store message to DB
            var result = await messageService.AddAsync(new MessageAddDTO()
            {
                ChatGroupId = chatGroupId.Value,
                Content = content
            },
                currUserId.Value);
            
            if (!result.IsSuccess) return;

            // send full DTO to receiver
            await Clients.Group(chatGroupId.Value.ToString()).ReceiveMessageAsync(result.Value!);
        }

        public async Task NotifyTyping()
        {
            var chatGroupId = await GetChatGroupIdAsync();
            if (chatGroupId == null) return;

            // if user switched to another chat group, remove previous typing status
            _usersTyping.AddOrUpdate(Context.ConnectionId, chatGroupId.Value, (_, __) => chatGroupId.Value);

            // add user to users typing dictionary with current chat group id
            await Clients.OthersInGroup(chatGroupId.Value.ToString()).UserIsTypingAsync(chatGroupId.Value);
        }

        public async Task NotifyStoppedTyping()
        {
            await ResolveStoppedTyping();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await ResolveStoppedTyping();

            await base.OnDisconnectedAsync(exception);
        }
        
        
        // helper methods
        
        // this can be used with users only
        private async Task<Guid?> GetUserChatGroupId()
        {
            var currUserId = GetUserId();
            if (currUserId == null) return null;

            var getCurrUserChatGroupIdResult = await chatGroupService.GetChatGroupIdAsync(currUserId.Value);
            if (!getCurrUserChatGroupIdResult.IsSuccess) return null;

            return getCurrUserChatGroupIdResult.Value;
        }

        private bool IsAdmin() =>
            Context.User?.IsInRole(nameof(RolesEnum.Admin)) ?? false;

        private async Task ResolveStoppedTyping()
        {
            // get current chatGroupId user is typing in
            // if user isnt typing stop
            if (!_usersTyping.TryRemove(Context.ConnectionId, out Guid chatGroupId)) return;


            List<string> currChatGroupUsersTyping = _usersTyping
                .Where(kvp => kvp.Value == chatGroupId)
                .Select(kvp => kvp.Key).ToList();
            
            int numberOfUsersTyping = currChatGroupUsersTyping.Count;

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

        private Guid? GetChatGroupId()
        {
            var context = httpContextAccessor.HttpContext;
            if(context == null) return  null;

            if (!context.Request.Query.TryGetValue("chatGroupId", out var chatGroupIdString))
                return null;

            if (!Guid.TryParse(chatGroupIdString, out var chatGroupId))
                return null;

            return chatGroupId;
        }

        private Guid? GetUserId()
        {
            var context = httpContextAccessor.HttpContext;
            if(context == null) return  null;

            var result = context.User.GetId();
            if (!result.IsSuccess) return null;
            
            return result.Value;
        }

        private async Task<Guid?> GetChatGroupIdAsync()
        {
            Guid? chatGroupId;
            if (IsAdmin()) chatGroupId = GetChatGroupId();
            else chatGroupId = await GetUserChatGroupId();
            
            return chatGroupId;
        }
    }
}