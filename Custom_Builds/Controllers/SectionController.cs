using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ISectionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    // only admins allowed
    [Authorize(Roles = nameof(RoleEnums.Admin))]
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {

        private readonly IAddSectionService _addSectionService;
        private readonly IEditSectionService _editSectionService;
        private readonly IRemoveSectionService _removeSectionService;

        public SectionController(
            IAddSectionService addSectionService,
            IEditSectionService editSectionService,
            IRemoveSectionService removeSectionService)
        {
            _addSectionService = addSectionService;
            _editSectionService = editSectionService;
            _removeSectionService = removeSectionService;
        }


        // add section
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] AddSectionDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addSectionService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        // edit section
        [HttpPut("[action]")]
        public async Task<IActionResult> Edit([FromBody] EditSectionDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _editSectionService.EditByIdAsync(newData);

            return result.ToActionResult();
        }

        // remove section
        [HttpDelete("[action]/{sectionId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid sectionId)
        {
            var result = await _removeSectionService.RemoveByIdAsync(sectionId);

            return result.ToActionResult();
        }

        // link midification with section
        [HttpPut]
        public async Task<IActionResult> LinkModification(LinkModificationDTO linkData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }


            Result result = await _editSectionService.LinkModification(linkData);

            return result.ToActionResult();
        }
    }
}