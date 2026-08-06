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
        public async Task<IActionResult> Remove(Guid id)
        {
            Result result = await productService.RemoveByIdAsync(id);
            
            return result.ToActionResult();
        }
    }
}
