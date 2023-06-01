namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction
{
    public class BchVoutResponse
    {
        public int Value { get; set; }
        public int N { get; set; }
        public List<BchAddressResponse> Addresses { get; set; }

    }
}