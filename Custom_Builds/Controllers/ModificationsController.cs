using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.ModificationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModificationsController : ControllerBase
    {
        private readonly IAddModificationService _addModificationService;
        private readonly IRemoveModificationService _removeModificationService;
        private readonly IEditModificationService _editModificationService;
        private readonly IGetModificationService _getModificationService;

        public ModificationsController(
            IAddModificationService addModificationService,
            IRemoveModificationService removeModificationService,
            IEditModificationService editModificationService,
            IGetModificationService getModificationService)
        {
            _addModificationService = addModificationService;
            _removeModificationService = removeModificationService;
            _editModificationService = editModificationService;
            _getModificationService = getModificationService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddModificationDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();

                return BadRequest(errors);
            }

            Result result = await _addModificationService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid modificationId)
        {
            var result = await _removeModificationService.RemoveByIdAsync(modificationId);

            return result.ToActionResult();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditModificationDTO newData)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();

                return BadRequest(errors);
            }

            Result result = await _editModificationService.EditByIdAsync(newData);

            return result.ToActionResult();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Modification>> Get(Guid modificationId)
        {
            var result = await _getModificationService.GetFromIdAsync(modificationId);

            return result.ToActionResult();
        }
    }
}
