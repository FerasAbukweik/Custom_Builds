using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {

        private readonly IGetFieldService _getFieldService;
        private readonly IAddFieldService _addFieldService;
        private readonly IEditFieldService _editFieldService;
        private readonly IRemoveFieldService _removeFieldService;

        public FieldController(
            IGetFieldService getFieldService,
            IAddFieldService addFieldService,
            IEditFieldService editFieldService,
            IRemoveFieldService removeFieldService)
        {
            _getFieldService = getFieldService;
            _addFieldService = addFieldService;
            _editFieldService = editFieldService;
            _removeFieldService = removeFieldService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Field>> Get(Guid fieldId)
        {
            var result = await _getFieldService.GetByIdAsync(fieldId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddFieldDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _addFieldService.AddAsync(toAdd);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditFieldDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _editFieldService.EditByIdAsync(newData);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid fieldId)
        {
            var result = await _removeFieldService.RemoveByIdAsync(fieldId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }
    }
}

