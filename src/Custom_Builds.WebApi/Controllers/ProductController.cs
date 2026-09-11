using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Product;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    public class ProductController(
        IProductService productService
        ) : ApplicationControllerBase
    {
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetAll([FromQuery] LazyDTO lazyData)
        {
            var result = await productService.LazyGetAllAsync(lazyData);

            return result.ToActionResult();
        }
        
        [HttpDelete("[action]/{id:guid}")]
        [Transactional]
        public async Task<IActionResult> Remove(Guid id)
        {
            Result result = await productService.RemoveByIdAsync(id);
            
            return result.ToActionResult();
        }

        [HttpPost("[action]")]
        [Transactional]
        public async Task<ActionResult<Guid>> Add([FromForm] ProductAddDTO productDto, CancellationToken cancellationToken = default)
        {
            var result = await productService.AddAsync(productDto, cancellationToken);

            if (!result.IsSuccess)
                return ((Result)result).ToActionResult();
            
            if(result.Value == null)
                return BadRequest("something went wrong");
            
            return Ok(result.Value.Id);
        }

        [HttpPut("[action]")]
        [Transactional]
        public async Task<IActionResult> Edit([FromBody]ProductEditDTO editData, CancellationToken cancellationToken = default)
        {
            Result result = await productService.EditAsync(editData, cancellationToken);

            return result.ToActionResult();
        }
    }
}
