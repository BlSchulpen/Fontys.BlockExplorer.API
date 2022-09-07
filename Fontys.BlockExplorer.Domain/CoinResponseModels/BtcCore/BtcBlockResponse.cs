using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
using Fontys.BlockExplorer.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace Fontys.BlockExplorer.Domain.NodeModels.BtcCore
{
    public class BtcBlockResponse
    {
        public string Hash { get; set; }
        public int Height { get; set; }
        public string? Previousblockhash { get; set; }
        public virtual ICollection<BtcTransactionResponse> Tx { get; set; }
    }
}
