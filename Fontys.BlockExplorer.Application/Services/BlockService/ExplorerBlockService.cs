using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Fontys.BlockExplorer.Application.Services.BlockService
{
    public class ExplorerBlockService : IBlockService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        private readonly IAddressRestoreService _addressRestoreService;
        private readonly ILogger<ExplorerBlockService> _logger;

        public ExplorerBlockService(BlockExplorerContext blockExplorerContext, IAddressRestoreService addressRestoreService, ILogger<ExplorerBlockService> logger)
        {
            _blockExplorerContext = blockExplorerContext;
            _addressRestoreService = addressRestoreService;
            _logger = logger;
        }

        public async Task<Block?> GetBlockAsync(GetBlockCommand getBlockCommand)
        {
            var hash = getBlockCommand.Hash;
            if (await _blockExplorerContext.Blocks.FirstOrDefaultAsync(b => b.Hash == getBlockCommand.Hash) == null) //TODO
            {
                return null;
            }

            var stored = await _blockExplorerContext.Blocks
                .Include(b => b.Transactions)
                .ThenInclude(t => t.Inputs)!
                        .ThenInclude(i => i.Address)
                .Include(b => b.Transactions)
                    .ThenInclude(t => t.Outputs)
                        .ThenInclude(o => o.Address)
                .FirstOrDefaultAsync(b => b.Hash == hash);
            return stored;
        }

        public async Task AddBlockAsync(Block block)
        {
            await _addressRestoreService.RestoreAddressesAsync(block);
            
            _logger.LogInformation("Storing blocks: {Block}", block);
            try
            {
                _blockExplorerContext.Add(block);
                await _blockExplorerContext.SaveChangesAsync();
                _logger.LogInformation("Stored blocks successfully");
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to store blocks, the following exception was thrown {Exception}", exception);
            }
        }

        public async Task RemoveBlocksAsync(List<Block> blocks)
        {
            _logger.LogInformation("Removing blocks: {Block}", blocks);
            try
            {
                _blockExplorerContext.Remove(blocks);
                await _blockExplorerContext.SaveChangesAsync();
                _logger.LogInformation("Removed blocks successfully");
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to remove blocks, the following exception was thrown {Exception}", exception);
            }
        }
    }
}
