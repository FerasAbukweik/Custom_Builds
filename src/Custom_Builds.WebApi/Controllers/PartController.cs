using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Part;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    public class PartController(
        IPartService partService
        ) : ApplicationControllerBase
    {
        // add part
        //[Authorize(Roles = nameof(RoleEnums.Admin))]
        [Authorize(Roles = nameof(RolesEnum.Admin))]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] PartAddDTO toPartAdd)
        {
            Result result = await partService.AddAsync(toPartAdd);

            return result.ToActionResult();
        }

        // remove part
        [Authorize(Roles = nameof(RolesEnum.Admin))]
        [HttpDelete("[action]/{partId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid partId)
        {
            Result result = await partService.RemoveByIdAsync(partId);

            return result.ToActionResult();
        }

        // allow normal users to get parts
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<PartDTO>>> GetAllParts()
        {
            var result = await partService.GetAllPartsIncludingAllData();

            return result.ToActionResult();
        }
    }
}
