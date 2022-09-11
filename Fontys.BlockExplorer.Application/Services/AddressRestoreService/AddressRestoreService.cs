using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Fontys.BlockExplorer.Application.Services.AddressRestoreService
{
    public class AddressRestoreService : IAddressRestoreService
    {
        private readonly BlockExplorerContext _context;

        public AddressRestoreService(BlockExplorerContext context)
        { 
            _context = context;
        }

        //TODO check on network type not cointype ---> for example btc cash 
        //TODO update this to make it way more readable
        public async Task RestoreAddressesAsync(Block block)
        {
            var addresses = new List<Address>();
            var inputAddresses = block.Transactions.Where(t => t.Inputs != null).SelectMany(tx => tx.Inputs).Where(i => i.Address != null).ToList().Select(i => i.Address).ToList();
            var outputAddresses = block.Transactions.Where(t => t.Outputs != null).SelectMany(tx => tx.Outputs).Where(i => i.Address != null).ToList().Select(i => i.Address).ToList();
            addresses.AddRange(inputAddresses);
            addresses.AddRange(outputAddresses);
            var distinctAddresses = addresses
                .GroupBy(i => i.Hash)
                .Select(i => i.First())
                .ToList();
            var distinctAddressesHashes = distinctAddresses.Select(a => a.Hash).ToList();
            var storedAddresses = _context.Addresses.Where(a => distinctAddressesHashes.Contains(a.Hash)).ToList();
            var storedAddressesHashes = storedAddresses.Select(a => a.Hash);
            var newAddresses = addresses.GroupBy(x => x.Hash).Select(y => y.First()).ToList();
            foreach (var a in newAddresses)
            {
                _context.Addresses.Add(a);
                await _context.SaveChangesAsync();
            }
            foreach (var input in block.Transactions.ToList().SelectMany(t => t.Inputs).Where(a => a.Address != null))
            {
                input.Address = _context.Addresses.FirstOrDefault(x => x.Hash == input.Address.Hash);
            }
            block.Transactions.ToList().ForEach(t => t.Inputs.ToList().ForEach(input => input.Address = _context.Addresses.FirstOrDefault(a => a.Hash == input.Address.Hash)));
        }
    }
}
