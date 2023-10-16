using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _db;

        public ProductController(DataContext dataContext)
        {
            _db = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _db.Products.ToListAsync();
            return Ok(products);
        }
    }
}