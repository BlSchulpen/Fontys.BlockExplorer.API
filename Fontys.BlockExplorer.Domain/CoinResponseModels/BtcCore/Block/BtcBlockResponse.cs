namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block
{
    public class BtcBlockResponse
    {
        public string Hash { get; set; }
        public int Height { get; set; }
        public string? Previousblockhash { get; set; }
        public virtual ICollection<BtcBlockTxResponse> Transactions { get; set; }
    }
}
