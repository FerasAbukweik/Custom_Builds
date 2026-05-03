using Custom_Builds.Core.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult ReceaiveMessage(SentMessageDTO toSend)
        {
            return Ok();
        }
    }
}
