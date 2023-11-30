using Shared;

namespace Server.Services.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
        Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync();
        Task<ServiceResponse<List<Category>>> AddCategoryAsync(Category category);
        Task<ServiceResponse<List<Category>>> UpdateCategoryAsync(Category category);
        Task<ServiceResponse<List<Category>>> DeleteCategoryAsync(int id);
        Task<ServiceResponse<Category>> GetCategoryAsync(int categoryId);
    }
}