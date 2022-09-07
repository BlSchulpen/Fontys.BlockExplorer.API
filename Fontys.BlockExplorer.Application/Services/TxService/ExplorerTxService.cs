using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Fontys.BlockExplorer.Application.Services.TxService
{
    public class ExplorerTxService : ITxService
    {
        private readonly BlockExplorerContext _blockExplorerContext;

        public ExplorerTxService(BlockExplorerContext blockExplorerContext)
        {
            _blockExplorerContext = blockExplorerContext;
        }

        public async Task<Transaction?> GetTransactionAsync(GetTxCommand command)
        {
            var hash = command.Hash;
            var stored = await _blockExplorerContext.Transactions.FirstOrDefaultAsync(b => b.Hash == hash);
            return stored;
        }
    }
}
