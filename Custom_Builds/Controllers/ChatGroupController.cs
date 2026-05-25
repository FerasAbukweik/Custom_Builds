using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts.IChatGroupServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGroupController : ControllerBase
    {
        private readonly IGetCurrUserService _getCurrUserService;
        private readonly IGetChatGroupService _getChatGroupService;

        public ChatGroupController(IGetCurrUserService getCurrUserService,
                                   IGetChatGroupService getChatGroupService)
        {
            _getCurrUserService = getCurrUserService;
            _getChatGroupService = getChatGroupService;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<InitChatGroupDataDTO>> GetInitChatGroupData()
        {
            var getCurrUserId = _getCurrUserService.GetUserId();
            if (!getCurrUserId.IsSuccess) return BadRequest(getCurrUserId.ErrorMessage ?? "cannt get user id");

            var getChatGroupId = await _getChatGroupService.GetChatGroupId(getCurrUserId.Value);
            if (!getChatGroupId.IsSuccess) return BadRequest(getChatGroupId.ErrorMessage ?? "cannt get chatGroup id");

            var initChatGroupData = new InitChatGroupDataDTO
            {
                UserId = getCurrUserId.Value,
                ChatGroupId = getChatGroupId.Value
            };

            return Ok(initChatGroupData);
        }
    }
}
