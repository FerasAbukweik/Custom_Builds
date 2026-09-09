using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Section;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Authorize(Roles = nameof(RolesEnum.Admin))]
    public class SectionController(
        ISectionService sectionService
        ) : ApplicationControllerBase
    {
        // add section
        [HttpPost("[action]")]
        public async Task<ActionResult<SectionDTO>> Add([FromBody] SectionAddDTO toSectionAdd)
        {
            var result = await sectionService.AddAsync(toSectionAdd);

            return result.ToActionResult();
        }

        // remove section
        [HttpDelete("[action]/{sectionId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid sectionId)
        {
            Result result = await sectionService.RemoveByIdAsync(sectionId);

            return result.ToActionResult();
        }
    }
}