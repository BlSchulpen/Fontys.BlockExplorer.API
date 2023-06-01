namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block
{
    public class BchBlockResponse
    {
        public string Hash { get; set; }

        public List<BchBlockTxResponse> Tx { get; set; }
    }
}
