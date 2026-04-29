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
    public class ItemsController : ControllerBase
    {
        private readonly IAddItemService _addItemService;
        private readonly IRemoveItemService _removeItemService;
        private readonly IEditItemService _editItemService;
        private readonly IGetItemService _getItemService;

        public ItemsController(
            IAddItemService addItemService,
            IRemoveItemService removeItemService,
            IEditItemService editItemService,
            IGetItemService getItemService)
        {
            _addItemService = addItemService;
            _removeItemService = removeItemService;
            _editItemService = editItemService;
            _getItemService = getItemService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddItemDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();

                return BadRequest(errors);
            }

            var result = await _addItemService.AddAsync(toAdd);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid itemId)
        {
            var result = await _removeItemService.RemoveByIdAsync(itemId);
            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return result.ToActionResult();
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditItemDTO newData)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();

                return BadRequest(errors);
            }

            var result = await _editItemService.EditByIdAsync(newData);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Item>> Get(Guid itemId)
        {
            var result = await _getItemService.GetFromIdAsync(itemId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok(result.Value);
        }
    }
}
