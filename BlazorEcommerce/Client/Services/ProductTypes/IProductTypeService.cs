using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.ProductTypes
{
    public interface IProductTypeService
    {
        event Action OnChange;
        public List<ProductType> ProductTypes { get; set; }
        Task GetProductTypesAsync();
    }
}