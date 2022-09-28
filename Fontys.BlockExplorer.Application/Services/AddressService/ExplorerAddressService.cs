using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Fontys.BlockExplorer.Application.Services.AddressService
{
    public class ExplorerAddressService : IAddressService
    {
        private readonly BlockExplorerContext _blockExplorerContext;

        public ExplorerAddressService(BlockExplorerContext blockExplorerContext)
        {
            _blockExplorerContext = blockExplorerContext;
        }

        public async Task<Address?> GetAddressAsync(GetAddressCommand getAddressCommand)
        {
            var hash = getAddressCommand.Hash;
            var stored = await _blockExplorerContext.Addresses.FirstOrDefaultAsync(b => b.Hash == hash);
            return stored;
        }
    }
}
