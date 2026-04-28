using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {

        private readonly IFieldRepository _fieldsRepository;

        public FieldController(IFieldRepository fieldsRepository)
        {
            _fieldsRepository = fieldsRepository;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Field>> Get(Guid fieldId)
        {
            var result = await _fieldsRepository.GetByIdAsync(fieldId);

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

            var result = await _fieldsRepository.AddAsync(toAdd);

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

            var result = await _fieldsRepository.EditByIdAsync(newData);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid fieldId)
        {
            var result = await _fieldsRepository.RemoveByIdAsync(fieldId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }
    }
}

