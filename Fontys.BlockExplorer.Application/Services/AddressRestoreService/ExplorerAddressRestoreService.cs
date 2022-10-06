using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using System.Linq;

namespace Fontys.BlockExplorer.Application.Services.AddressRestoreService
{
    public class ExplorerAddressRestoreService : IAddressRestoreService
    {
        private readonly BlockExplorerContext _context;

        public ExplorerAddressRestoreService(BlockExplorerContext context)
        {
            _context = context;
        }

        //TODO Get all addresses that have been stored yet and just assign the context address
            // Issue => sometimes new addresses are in multiple transacions this causes an dupplicate primary key error when adding the block in the DB 

        public async Task<List<Address>> RestoreAddressesAsync(Block block)
        {
            var addresses = new List<Address>();
            var inputAddresses = block.Transactions.Where(t => t.Inputs != null).SelectMany(tx => tx.Inputs)
                .Where(i => i.Address != null).ToList().Select(i => i.Address).ToList();
            var outputAddresses = block.Transactions.Where(t => t.Outputs != null).SelectMany(tx => tx.Outputs)
                .Where(i => i.Address != null).ToList().Select(i => i.Address)
                .ToList(); 
            addresses.AddRange(inputAddresses);
            addresses.AddRange(outputAddresses);
            var hashes = addresses.Select(a => a.Hash).ToList();
            var alreadyStoredAddresses = _context.Addresses.Where(a => hashes.Contains(a.Hash)).ToList();
            var alreadyStoredAddressesHashes = alreadyStoredAddresses.Select(a => a.Hash).ToList();
            var newAddressesHashes = hashes.Except(alreadyStoredAddressesHashes).ToList();
            var newAddresses = addresses.Where(a => newAddressesHashes.Contains(a.Hash)).ToList();
            var distinctNewAddresses = newAddresses
                .GroupBy(i => i.Hash)
                .Select(i => i.First())
                .ToList();
            _context.Addresses.AddRange(distinctNewAddresses); 
            await _context.SaveChangesAsync(); 


           var blockStoredAddresses = _context.Addresses.Where(x => hashes.Contains(x.Hash));
           var inputs = block.Transactions.ToList().SelectMany(t => t.Inputs).Where(a => a.Address != null).ToList();
           var outputs = block.Transactions.ToList().SelectMany(t => t.Outputs).Where(a => a.Address != null).ToList();


           //Think this mostly solves the performance issues (from ~7sec to ~2secs)
            foreach (var address in blockStoredAddresses)
            {
                inputs.Where(x => x.Address.Hash == address.Hash).ToList().ForEach(i => i.Address = address);
                outputs.Where(x => x.Address.Hash == address.Hash).ToList().ForEach(i => i.Address = address);
            }

            /*
            foreach (var input in block.Transactions.ToList().SelectMany(t => t.Inputs).Where(a => a.Address != null))
            {
                input.Address = _context.Addresses.FirstOrDefault(x => x.Hash == input.Address.Hash);
            }

            foreach (var output in block.Transactions.ToList().SelectMany(t => t.Outputs).Where(a => a.Address != null))
            {
                output.Address = _context.Addresses.FirstOrDefault(x => x.Hash == output.Address.Hash);
            }
            */

            return distinctNewAddresses;
        }
    }
}
