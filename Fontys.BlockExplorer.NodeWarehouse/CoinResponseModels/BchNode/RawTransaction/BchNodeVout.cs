namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction
{
    public class BchNodeVout
    {
        public int Value { get; set; }
        public int N { get; set; }
        public List<BchNodeAddress> Addresses { get; set; } 

    }
}