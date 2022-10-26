namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block
{
    public class BchNodeBlock
    {
        public string Hash { get; set; }

        public List<BchNodeBlockTransaction> Tx { get; set; }
    }
}
