using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Fontys.BlockExplorer.Application.Services.AddressService
{
    public class ExplorerAddressService : IAddressService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        private readonly ILogger<ExplorerAddressService> _logger; //TODO maybe logging to much?

        public ExplorerAddressService(BlockExplorerContext blockExplorerContext, ILogger<ExplorerAddressService> logger)
        {
            _blockExplorerContext = blockExplorerContext;
            _logger = logger;
        }

        public async Task StoreAddressesAsync(List<Address> addresses)
        {   
            _logger.LogInformation("Storing the following addresses: {addresses}", addresses);
            try
            {
                _blockExplorerContext.Addresses.AddRange(addresses);
                await _blockExplorerContext.SaveChangesAsync();
                _logger.LogInformation("Stored addresses successfully");
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to store addresses, the following exception was thrown {Exception}", exception);
            }
        }

        public async Task<Address?> GetAddressAsync(GetAddressCommand getAddressCommand)
        {
            var hash = getAddressCommand.Hash;
            _logger.LogInformation("Retrieving address with hash {Hash}", hash);
            try
            {
                var stored = await _blockExplorerContext.Addresses.FirstOrDefaultAsync(b => b.Hash == hash);
                _logger.LogInformation("Retrieved address is: {Address}", stored);
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
