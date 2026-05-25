using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts.IProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGetProductService _getProductService;

        public ProductController(IGetProductService getProductService)
        {
            _getProductService = getProductService;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<List<ProductDTO>>> GetAll([FromQuery] LazyDTO reqData)
        {
            var result = await _getProductService.GetAllAsync(reqData);

            return result.ToActionResult();
        }
    }
}
