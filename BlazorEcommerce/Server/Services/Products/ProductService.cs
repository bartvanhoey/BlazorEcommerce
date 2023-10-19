using System.Reflection.Metadata.Ecma335;
using Azure;
using Server.Data;
using Shared;

namespace Server.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DataContext _db;

        public ProductService(DataContext dataContext) => _db = dataContext;

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var product = await _db.Products.Include(p => p.Variants).ThenInclude(v => v.ProductType).FirstOrDefaultAsync(p => p.Id == productId);
            var response = new ServiceResponse<Product>();
            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
            }

            return response;



        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
            => new ServiceResponse<List<Product>>() { Data = await _db.Products.Include(p => p.Variants).ToListAsync() };

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _db.Products.Where(p => p.Category != null && p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).Include(p => p.Variants).ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySearchText(searchText);
            var result = new List<string>();

            foreach (var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if (product.Description != null)
                {
                    var punctuations = product.Description.Where(char.IsPunctuation).Distinct().ToArray();
                    var words = product.Description.Split().Select(s => s.Trim(punctuations));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }

                }

            }




            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<List<Product>>> SearchProductsAsync(string searchText)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await FindProductsBySearchText(searchText)
            };

            return response;
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _db.Products
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                                || p.Description.ToLower().Contains(searchText.ToLower()))
                                .Include(p => p.Variants).ToListAsync();
        }
    }
}