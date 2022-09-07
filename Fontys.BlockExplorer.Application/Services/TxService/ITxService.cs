using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.Application.Services.TxService
{
    public interface ITxService
    {
        public Task<Transaction?> GetTransactionAsync(GetTxCommand command);
    }
}
