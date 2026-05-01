using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IPartServices;
using Custom_Builds.Core.ServiceContracts.PartServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IGetPartService _getPartService;
        private readonly IAddPartService _addPartService;
        private readonly IEditPartService _editPartService;
        private readonly IRemovePartService _removePartService;

        public PartController(
            IGetPartService getPartService,
            IAddPartService addPartService,
            IEditPartService editPartService,
            IRemovePartService removePartService)
        {
            _getPartService = getPartService;
            _addPartService = addPartService;
            _editPartService = editPartService;
            _removePartService = removePartService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Part>> Get(Guid partId)
        {
            var result = await _getPartService.GetByIdAsync(partId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddPartDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addPartService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditPartDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _editPartService.EditByIdAsync(newData);

            return result.ToActionResult();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid partId)
        {
            Result result = await _removePartService.RemoveByIdAsync(partId);

            return result.ToActionResult();
        }
    }
}
