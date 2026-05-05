using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IPartServices;
using Custom_Builds.Core.ServiceContracts.PartServices;
using Microsoft.AspNetCore.Authorization;
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



        // add part
        [Authorize(Roles = nameof(RoleEnums.Admin))]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] AddPartDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addPartService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        // edit part
        [Authorize(Roles = nameof(RoleEnums.Admin))]
        [HttpPut("[action]")]
        public async Task<IActionResult> Edit([FromBody] EditPartDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _editPartService.EditByIdAsync(newData);

            return result.ToActionResult();
        }

        // remove part
        [Authorize(Roles = nameof(RoleEnums.Admin))]
        [HttpDelete("[action]/{partId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid partId)
        {
            Result result = await _removePartService.RemoveByIdAsync(partId);

            return result.ToActionResult();
        }

        // allow normal users to use this
        [HttpGet("[action]")]
        public async Task<ActionResult<List<Part>>> GetAllParts()
        {
            var result = await _getPartService.GetAllPartsIncludingAllData();

            return result.ToActionResult();
        }
    }
}
