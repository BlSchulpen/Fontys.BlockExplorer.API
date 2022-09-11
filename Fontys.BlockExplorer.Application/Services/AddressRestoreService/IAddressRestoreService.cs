using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.AddressRestoreService
{
    public interface IAddressRestoreService
    {
        Task<List<Address>> RestoreAddressesAsync(Block block);
    }
}
