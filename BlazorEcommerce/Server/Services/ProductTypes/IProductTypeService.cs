using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Data;
using Shared;

namespace Server.Services.ProductTypes
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync();
    }

    public class ProductTypeService : IProductTypeService
    {
        private readonly DatabaseContext _context;

        public ProductTypeService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypesAsync()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>> { Data = productTypes };
        }
    }
}