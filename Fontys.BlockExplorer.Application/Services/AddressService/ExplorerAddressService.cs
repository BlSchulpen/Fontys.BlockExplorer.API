namespace Fontys.BlockExplorer.Application.Services.AddressService
{
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class ExplorerAddressService : IAddressService
    {
        private readonly BlockExplorerContext _blockExplorerContext;

        public ExplorerAddressService(BlockExplorerContext blockExplorerContext)
        {
            _blockExplorerContext = blockExplorerContext;
        }

        public async Task<Address?> GetAddressAsync(GetAddressCommand command)
        {
            var hash = command.Hash;
            var stored = await _blockExplorerContext.Addresses.FirstOrDefaultAsync(b => b.Hash == hash);
            return stored;
        }
    }
}
