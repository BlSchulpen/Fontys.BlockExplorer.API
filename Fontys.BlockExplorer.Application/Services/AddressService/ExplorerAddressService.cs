using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Fontys.BlockExplorer.Application.Services.AddressService
{
    public class ExplorerAddressService : IAddressService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        private readonly ILogger<ExplorerAddressService> _logger; 

        public ExplorerAddressService(BlockExplorerContext blockExplorerContext, ILogger<ExplorerAddressService> logger)
        {
            _blockExplorerContext = blockExplorerContext;
            _logger = logger;
        }

        public async Task StoreAddressesAsync(List<Address> addresses)
        {   
            try
            {
                _blockExplorerContext.Addresses.AddRange(addresses);
                await _blockExplorerContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to store addresses, the following exception was thrown {Exception}", exception);
            }
        }

        public async Task<Address?> GetAddressAsync(string hash)
        {
            try
            {
                var stored = await _blockExplorerContext.Addresses.FirstOrDefaultAsync(b => b.Hash == hash);
                return stored;
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to retrieve address, the following exception was thrown {Exception}", exception);
                return null; //TODO handle
            }
        }
    }
}
