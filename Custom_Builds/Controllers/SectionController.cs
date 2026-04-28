using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionRepository _sectionsRepository;

        public SectionController(ISectionRepository sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Section>> Get(Guid sectionId)
        {
            var result = await _sectionsRepository.GetByIdAsync(sectionId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddSectionDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _sectionsRepository.AddAsync(toAdd);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditSectionDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _sectionsRepository.EditByIdAsync(newData);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid sectionId)
        {
            var result = await _sectionsRepository.RemoveByIdAsync(sectionId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }
    }
}
