using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http) => _http = http;

        public event Action OnChange;

        public async Task AddCategoryAsync(Category category)
        {
            var response = await _http.PostAsJsonAsync("api/category/admin", category);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>();
            OnChange();
        }

        public Category CreateNewCategory()
        {
            var newCategory = new Category{IsNew = true, Editing = true};
            return newCategory;
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _http.DeleteAsync($"api/category/admin/{categoryId}");
            OnChange();
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category/admin");
            if (response != null) return response;
            return new ServiceResponse<List<Category>> { Data = new List<Category>() };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (response != null) return response;
            return new ServiceResponse<List<Category>> { Data = new List<Category>() };
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var response = await _http.PutAsJsonAsync("api/category/admin", category);
            OnChange();
        }
    }
}