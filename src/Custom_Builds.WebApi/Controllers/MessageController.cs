using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Message;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{ 
    [Authorize]
    public class MessageController(
        IMessageService messageService,
        IChatGroupService chatGroupService
        ) : ApplicationControllerBase
    {
        // get messages
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<MessageDTO>>> GetMessages([FromQuery] LazyDTO lazyLoadData, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var chatGroupId = await chatGroupService.GetChatGroupIdAsync(getCurrUserId.Value, cancellationToken);
            if (!chatGroupId.IsSuccess) return ((Result)chatGroupId).ToActionResult();
            
            var result = await messageService.GetGroupMessagesAsync(chatGroupId.Value, getCurrUserId.Value, lazyLoadData, cancellationToken);

            return result.ToActionResult();
        }
    }
}