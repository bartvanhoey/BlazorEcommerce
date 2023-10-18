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


        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (response != null) return response;
            return new ServiceResponse<List<Category>> { Data = new List<Category>() };
        }
    }
}