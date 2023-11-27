using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Addresses
{
    public interface IAddressService
    {
        Task<Address> GetAddressAsync();
        Task<Address> AddOrdUpdateAddressAsync(Address address);
    }
}