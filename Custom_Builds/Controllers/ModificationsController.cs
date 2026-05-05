using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.ModificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    // only admin allowed
    [Authorize(Roles = nameof(RoleEnums.Admin))]
    [Route("api/[controller]")]
    [ApiController]
    public class ModificationsController : ControllerBase
    {
        private readonly IAddModificationService _addModificationService;
        private readonly IRemoveModificationService _removeModificationService;
        private readonly IEditModificationService _editModificationService;

        public ModificationsController(
            IAddModificationService addModificationService,
            IRemoveModificationService removeModificationService,
            IEditModificationService editModificationService,
            IGetModificationService getModificationService)
        {
            _addModificationService = addModificationService;
            _removeModificationService = removeModificationService;
            _editModificationService = editModificationService;
        }



        // add modification
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody]AddModificationDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addModificationService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        // remove modification
        [HttpDelete("[action]/{modificationId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid modificationId)
        {
            Result result = await _removeModificationService.RemoveByIdAsync(modificationId);

            return result.ToActionResult();
        }

        // edit modification
        [HttpPut("[action]")]
        public async Task<IActionResult> Edit([FromBody]EditModificationDTO newData)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();

                return BadRequest(errors);
            }

            Result result = await _editModificationService.EditByIdAsync(newData);

            return result.ToActionResult();
        }
    }
}
