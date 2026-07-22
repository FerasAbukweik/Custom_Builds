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
        public async Task<ActionResult<List<ProductDTO>>> GetAll([FromQuery] LazyDTO reqData)
        {
            var result = await productService.GetAllAsync(reqData);

            return result.ToActionResult();
        }
    }
}
