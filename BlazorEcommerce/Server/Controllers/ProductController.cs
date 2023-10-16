using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services.Products;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var response = await _productService.GetProductsAsync();
            return Ok(response);
        }
    }
}