using Server.Data;
using Shared;

namespace Server.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext _db;

        public CategoryService(DatabaseContext context) => _db = context;

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync() => new ServiceResponse<List<Category>>() { Data = await _db.Categories.ToListAsync() };

        public async Task<ServiceResponse<Category>> GetCategoryAsync(int categoryId)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(p => p.Id == categoryId);
            var response = new ServiceResponse<Category>();
            if (category == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this Category does not exist.";
            }
            else
            {
                response.Data = category;
            }

            return response;


        }
    }
}