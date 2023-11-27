using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services.Addresses
{
    public interface IAddressService
    {
        Task<ServiceResponse<Address>> GetAddressAsync();
        Task<ServiceResponse<Address>> AddOrUpdateAddressAsync(Address address);
    }
}