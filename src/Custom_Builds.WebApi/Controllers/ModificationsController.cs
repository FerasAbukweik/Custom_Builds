using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Modification;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Authorize(Roles = nameof(RolesEnum.Admin))]
    public class ModificationsController(
        IModificationsService modificationsService
        ) : ApplicationControllerBase
    {
        // add modification
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody]ModificationAddDTO toModificationAdd, CancellationToken cancellationToken = default)
        {
            Result result = await modificationsService.AddAsync(toModificationAdd, cancellationToken);

            return result.ToActionResult();
        }

        // remove modification
        [HttpDelete("[action]/{modificationId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid modificationId)
        {
            Result result = await modificationsService.RemoveByIdAsync(modificationId);

            return result.ToActionResult();
        }
    }
}
