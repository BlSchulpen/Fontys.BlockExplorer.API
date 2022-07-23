namespace Fontys.BlockExplorer.Application.Services.TxService
{
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;

    public interface ITxService
    {
        public Task<Transaction> GetTransactionAsync(GetTxCommand command);
    }
}
