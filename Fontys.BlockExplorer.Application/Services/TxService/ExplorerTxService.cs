namespace Fontys.BlockExplorer.Application.Services.TxService
{
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class ExplorerTxService : ITxService
    {
        private readonly BlockExplorerContext _blockExplorerContext;

        public ExplorerTxService(BlockExplorerContext blockExplorerContext)
        {
            _blockExplorerContext = blockExplorerContext;
        }

        public async Task<Transaction> GetBlockAsync(GetTxCommand command)
        {
            var hash = command.Hash;
            var stored = await _blockExplorerContext.Transactions.FirstOrDefaultAsync(b => b.Hash == hash);
            return stored;
        }
    }
}
