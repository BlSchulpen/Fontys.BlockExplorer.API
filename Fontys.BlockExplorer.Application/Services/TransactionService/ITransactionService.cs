using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.TxService
{
    public interface ITransactionService
    {
        public Task<Transaction?> GetTransactionAsync(CoinType coinType, string hash);
        public Task<List<Transaction>> GetLatestTransactionsAsync(CoinType coinType);

    }
}
