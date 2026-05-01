using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBuildController : ControllerBase
    {
        private readonly IAddCustomBuildService _addCustomBuildService;

        public CustomBuildController(IAddCustomBuildService addCustomBuildService)
        {
            _addCustomBuildService = addCustomBuildService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddCustomBuildDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addCustomBuildService.AddByModificationsIdsAsync(toAdd.ModificationIds, toAdd.CustomBuildType);
            
            return result.ToActionResult();
        }
    }
}
