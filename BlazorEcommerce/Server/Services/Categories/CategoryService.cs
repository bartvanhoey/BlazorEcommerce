using Server.Data;
using Shared;

namespace Server.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext _db;

        public CategoryService(DatabaseContext context) => _db = context;

        public async Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category)
        {
            category.Editing = category.IsNew = false;
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int id)
        {
            var dbCategory = await GetCategoryFromDbById(id);
            if (dbCategory == null) return new ServiceResponse<List<Category>> { Success = false, Message = "Category NOT found" };
            dbCategory.Deleted = true;
            await _db.SaveChangesAsync();
            return await GetAdminCategoriesAsync();
        }

        private Task<Category?> GetCategoryFromDbById(int id)
        {
            return _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
            => new ServiceResponse<List<Category>>() { Data = await _db.Categories.Where(c => !c.Deleted).ToListAsync() };


        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
            => new ServiceResponse<List<Category>>() { Data = await _db.Categories.Where(c => !c.Deleted && c.Visible).ToListAsync() };

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

        public async Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category)
        {
            var dbCategory = await GetCategoryFromDbById(category.Id);
            if (dbCategory == null) return new ServiceResponse<List<Category>> { Success = false, Message = "Category NOT found" };
            
            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _db.SaveChangesAsync();
            
            return await GetAdminCategoriesAsync();
        }
    }
}