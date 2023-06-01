using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.TxService
{
    public class ExplorerTransactionService : ITransactionService
    {
        private readonly BlockExplorerContext _blockExplorerContext;
        private readonly ILogger<ExplorerTransactionService> _logger;

        public ExplorerTransactionService(BlockExplorerContext blockExplorerContext, ILogger<ExplorerTransactionService> logger)
        {
            _blockExplorerContext = blockExplorerContext;
            _logger = logger;
        }

        public async Task<List<Transaction>?> GetLatestTransactionsAsync(CoinType coinType)
        {
            _logger.LogInformation("Get latest transactions of coin: {CoinType}", coinType); //maybe only log errors...
            try
            {
                // somecoins might have less than 10 transactions in the latest block or no transactions at all ...
                const int numberOfBlocks = 10; //TODO consider how to indicate this...
                var latestTransactions = await _blockExplorerContext.Blocks.Where(b => b.CoinType == coinType).SelectMany(t => t.Transactions).Take(numberOfBlocks).ToListAsync();
                return latestTransactions;
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to retrieve transaction, the following exception was thrown {Exception}", exception);
                return null;
            }
        }

        public async Task<Transaction?> GetTransactionAsync(CoinType cointype, string hash)
        {
            _logger.LogInformation("Retrieving transaction with hash: {Hash}", hash); //maybe only log errors...
            try
            {
                var test = _blockExplorerContext.Blocks.ToList();
                var stored = await _blockExplorerContext.Blocks.Where(b => b.CoinType == cointype).Select(b => b.Transactions).Where(t => t.Any(x => x.Hash == hash)).Select(t => t.FirstOrDefault(t => t.Hash == hash)).FirstOrDefaultAsync();
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
