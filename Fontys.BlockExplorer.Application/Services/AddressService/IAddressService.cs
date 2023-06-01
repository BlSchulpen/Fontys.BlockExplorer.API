using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.AddressService
{
    public interface IAddressService
    {
        public Task<Address?> GetAddressAsync(string hash); //TODO remove or use CQS
        public Task StoreAddressesAsync(List<Address> addresses);
    }
}
