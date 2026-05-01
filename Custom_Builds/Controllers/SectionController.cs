using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ISectionServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {

        private readonly IGetSectionService _getSectionService;
        private readonly IAddSectionService _addSectionService;
        private readonly IEditSectionService _editSectionService;
        private readonly IRemoveSectionService _removeSectionService;

        public SectionController(
            IGetSectionService getSectionService,
            IAddSectionService addSectionService,
            IEditSectionService editSectionService,
            IRemoveSectionService removeSectionService)
        {
            _getSectionService = getSectionService;
            _addSectionService = addSectionService;
            _editSectionService = editSectionService;
            _removeSectionService = removeSectionService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Section>> Get(Guid sectionId)
        {
            var result = await _getSectionService.GetByIdAsync(sectionId);

            return result.ToActionResult();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddSectionDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addSectionService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditSectionDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _editSectionService.EditByIdAsync(newData);

            return result.ToActionResult();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid sectionId)
        {
            var result = await _removeSectionService.RemoveByIdAsync(sectionId);

            return result.ToActionResult();
        }
    }
}

