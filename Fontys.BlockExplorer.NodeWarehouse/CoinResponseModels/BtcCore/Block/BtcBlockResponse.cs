namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.Block
{
    public class BtcBlockResponse
    {
        public string Hash { get; set; }
        public int Height { get; set; }
        public string? Previousblockhash { get; set; }
        public virtual ICollection<BtcBlockTxResponse> Tx { get; set; }
    }
}
