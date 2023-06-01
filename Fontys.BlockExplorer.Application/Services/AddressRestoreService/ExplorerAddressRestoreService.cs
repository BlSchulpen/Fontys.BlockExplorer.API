using Fontys.BlockExplorer.Application.Services.AddressService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.AddressRestoreService
{
    public class ExplorerAddressRestoreService : IAddressRestoreService
    {
        private readonly BlockExplorerContext _context;
        private readonly IAddressService _addressService;
        private readonly ILogger<IAddressRestoreService> _logger;

        public ExplorerAddressRestoreService(BlockExplorerContext context, IAddressService addressService, ILogger<IAddressRestoreService> logger)
        {
            _addressService = addressService;
            _context = context;
            _logger = logger;
        }

        public async Task<List<Address>> RestoreAddressesAsync(Block block)
        {
            var addressesInBlock = GetAllAddressesOfBlock(block);
            var distinctNewAddresses = GetDistinctNewAddresses(addressesInBlock);
            if (distinctNewAddresses.Any())
                await StoreNewAddresses(distinctNewAddresses);
            var dbAddressesInBlock = _context.Addresses.Where(a => addressesInBlock.Contains(a)).ToList();
            UpdateTransferAddresses(block, dbAddressesInBlock);
            return distinctNewAddresses;
        }

        private async Task<List<Address>?> StoreNewAddresses(List<Address> distinctNewAddresses)
        {
            try
            {
                await _addressService.StoreAddressesAsync(distinctNewAddresses);
                return distinctNewAddresses;
            }

            catch (Exception exception)
            {
                _logger.LogError("Failed store new addresses, the following exception was thrown {Exception}", exception);
                return null;
            }
        }

        private List<Address> GetAllAddressesOfBlock(Block block)
        {
            var inputAddresses = block.Transactions.Where(t => t.Inputs != null).SelectMany(tx => tx.Inputs)
                .Where(i => i.Address != null).ToList().Select(i => i.Address).ToList();
            var outputAddresses = block.Transactions.Where(t => t.Outputs != null).SelectMany(tx => tx.Outputs)
                .Where(i => i.Address != null).ToList().Select(i => i.Address)
                .ToList();
            var addresses = new List<Address>(inputAddresses);
            addresses.AddRange(outputAddresses);
            return addresses;
        }

        private List<Address> GetDistinctNewAddresses(List<Address> addresses)
        {
            var hashes = addresses.Select(a => a.Hash).ToList();
            var alreadyStoredAddresses = _context.Addresses.Where(a => hashes.Contains(a.Hash)).ToList();
            var alreadyStoredAddressesHashes = alreadyStoredAddresses.Select(a => a.Hash).ToList();
            var newAddressesHashes = hashes.Except(alreadyStoredAddressesHashes).ToList();
            var newAddresses = addresses.Where(a => newAddressesHashes.Contains(a.Hash)).ToList();
            var distinctNewAddresses = newAddresses
                .GroupBy(i => i.Hash)
                .Select(i => i.First())
                .ToList();
            return distinctNewAddresses;
        }

        private static void UpdateTransferAddresses(Block block, List<Address> dbAddressesInBlock)
        {
            var inputs = block.Transactions.ToList().SelectMany(t => t.Inputs).Where(a => a.Address != null).ToList();
            var outputs = block.Transactions.ToList().SelectMany(t => t.Outputs).Where(a => a.Address != null).ToList();
            foreach (var address in dbAddressesInBlock)
            {
                inputs.Where(x => x.Address.Hash == address.Hash).ToList().ForEach(i => i.Address = address);
                outputs.Where(x => x.Address.Hash == address.Hash).ToList().ForEach(i => i.Address = address);
            }
        }
    }
}
