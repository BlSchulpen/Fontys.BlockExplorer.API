using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.TxService
{
    public interface ITxService
    {
        public Task<Transaction?> GetTransactionAsync(string hash);
    }
}
