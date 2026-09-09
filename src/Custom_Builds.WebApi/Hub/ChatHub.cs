using custom_Peripherals.IHub;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Custom_Builds.Core.DTO.Message;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using System.Security.Claims;

namespace custom_Peripherals.Hub
{
    public class ChatHub(
        IMessageService messageService,
        IChatGroupService chatGroupService
        ) : Hub<IChatHub>
    {
        private static readonly ConcurrentDictionary<string, Guid> _usersTyping = new();

        public override async Task OnConnectedAsync()
        {
            // Automatically add normal users to their specific chat group upon connection
            if (!IsAdmin())
            {
                var userGroupId = await GetUserChatGroupId();
                if (userGroupId.HasValue)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, userGroupId.Value.ToString());
                }
            }

            await base.OnConnectedAsync();
        }

        // ==========================================
        // Join and Leave methods (Crucial for Admin)
        // ==========================================
        public async Task JoinChatGroup(Guid chatGroupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId.ToString());
        }

        public async Task LeaveChatGroup(Guid chatGroupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatGroupId.ToString());
        }

        // ==========================================
        // Sending Messages
        // ==========================================
        
        // Added adminChatGroupId so the admin can specify the target chat group
        public async Task SendMessage(string content, Guid? adminChatGroupId = null)
        {
            var targetGroupId = await GetTargetChatGroupIdAsync(adminChatGroupId);
            var currUserId = GetUserId();

            if (targetGroupId == null || currUserId == null) return;

            // Save message to the database
            var result = await messageService.AddAsync(new MessageAddDTO()
            {
                ChatGroupId = targetGroupId.Value,
                Content = content
            }, currUserId.Value);

            if (!result.IsSuccess) return;

            // Send the DTO to everyone in the chat group
            await Clients.Group(targetGroupId.Value.ToString()).ReceiveMessageAsync(result.Value!);
        }

        // ==========================================
        // Typing Indicators
        // ==========================================
        public async Task NotifyTyping(Guid? adminChatGroupId = null)
        {
            var chatGroupId = await GetTargetChatGroupIdAsync(adminChatGroupId);
            if (chatGroupId == null) return;

            // Update or add the user's typing status
            _usersTyping.AddOrUpdate(Context.ConnectionId, chatGroupId.Value, (_, __) => chatGroupId.Value);

            // Notify others in the chat group that someone is typing
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

        // ==========================================
        // Helper Methods
        // ==========================================

        private async Task<Guid?> GetUserChatGroupId()
        {
            var currUserId = GetUserId();
            if (currUserId == null) return null;

            var result = await chatGroupService.GetChatGroupIdAsync(currUserId.Value);
            return result.IsSuccess ? result.Value : null;
        }

        private bool IsAdmin() =>
            Context.User?.IsInRole(nameof(RolesEnum.Admin)) ?? false;

        private async Task<Guid?> GetTargetChatGroupIdAsync(Guid? adminChatGroupId)
        {
            // If admin, use the provided ID; otherwise, use the user's own group to prevent tampering
            if (IsAdmin()) return adminChatGroupId;
            return await GetUserChatGroupId();
        }

        private Guid? GetUserId()
        {
            // Extract the ID directly from the SignalR Context without using HttpContextAccessor
            var userIdString = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (Guid.TryParse(userIdString, out var userId))
                return userId;

            return null;
        }

        private async Task ResolveStoppedTyping()
        {
            if (!_usersTyping.TryRemove(Context.ConnectionId, out Guid chatGroupId)) return;

            // Find who else is still typing in the same chat group
            var currChatGroupUsersTyping = _usersTyping
                .Where(kvp => kvp.Value == chatGroupId)
                .Select(kvp => kvp.Key)
                .ToList();

            int numberOfUsersTyping = currChatGroupUsersTyping.Count;

            if (numberOfUsersTyping == 0)
            {
                // No one is typing, notify everyone to hide the typing indicator
                await Clients.Group(chatGroupId.ToString()).UserStoppedTypingAsync(chatGroupId);
            }
            else if (numberOfUsersTyping == 1)
            {
                // If only one person is still typing, notify only them that the others stopped 
                // (Kept based on your original logic)
                await Clients.Client(currChatGroupUsersTyping[0]).UserStoppedTypingAsync(chatGroupId);
            }
        }
    }
}