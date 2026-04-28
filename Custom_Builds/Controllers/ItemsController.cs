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
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddItemDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();

                return BadRequest(errors);
            }

            var result = await _itemsRepository.AddAsync(toAdd);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid itemId)
        {
            var result = await _itemsRepository.RemoveByIdAsync(itemId);
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

            var result = await _itemsRepository.EditByIdAsync(newData);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Item>> Get(Guid itemId)
        {
            var result = await _itemsRepository.GetFromIdAsync(itemId);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok(result.Value);
        }
    }
}
