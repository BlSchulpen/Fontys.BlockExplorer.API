namespace Fontys.BlockExplorer.Application.Services.AddressService
{
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;

    public interface IAddressService
    {
        public Task<Address?> GetAddressAsync(GetAddressCommand getBlockCommand);
    }
}
