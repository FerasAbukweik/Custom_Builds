using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IGetMessageService _getMessagesService;
        public MessageController(IGetMessageService getMessagesService)
        {
            _getMessagesService = getMessagesService;
        }


        // get messages
        [HttpGet("[action]")]
        public async Task<ActionResult<List<MessageDTO>>> GetMessages([FromQuery] LazyDTO lazyLoadData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }
            var result = await _getMessagesService.GetMessagesAsync(lazyLoadData);

            return result.ToActionResult();
        }
    }
}