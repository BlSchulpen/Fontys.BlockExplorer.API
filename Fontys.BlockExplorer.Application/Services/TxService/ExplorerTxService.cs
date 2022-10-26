using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.TxService
{
    public class ExplorerTxService : ITxService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        private readonly ILogger<ExplorerTxService> _logger;

        public ExplorerTxService(BlockExplorerContext blockExplorerContext, ILogger<ExplorerTxService> logger)
        {
            _blockExplorerContext = blockExplorerContext;
            _logger = logger;
        }

        public async Task<Transaction?> GetTransactionAsync(string hash)
        {
            _logger.LogInformation("Retrieving transaction with hash: {Hash}", hash);
            try
            {
                var stored = await _blockExplorerContext.Transactions.FirstOrDefaultAsync(b => b.Hash == hash);
                _logger.LogInformation("Retrieved transaction is: {Transaction}", stored); 
                return stored;
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to retrieve transaction, the following exception was thrown {Exception}", exception);
                return null;
            }
        }
    }
}
