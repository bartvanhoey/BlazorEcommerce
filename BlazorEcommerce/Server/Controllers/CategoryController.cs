using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) 
            => _categoryService = categoryService;

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetProducts()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return Ok(result);
        }

            
    }
}