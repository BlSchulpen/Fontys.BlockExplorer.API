using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.AddressService
{
    public class ExplorerAddressService : IAddressService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        private ILogger<ExplorerAddressService> _logger;

        public ExplorerAddressService(BlockExplorerContext blockExplorerContext, ILogger<ExplorerAddressService> logger)
        {
            _blockExplorerContext = blockExplorerContext;
            _logger = logger;
        }

        public async Task<Address?> GetAddressAsync(GetAddressCommand getAddressCommand)
        {
            var hash = getAddressCommand.Hash;
            var stored = await _blockExplorerContext.Addresses.FirstOrDefaultAsync(b => b.Hash == hash);
            return stored;
        }
    }
}
