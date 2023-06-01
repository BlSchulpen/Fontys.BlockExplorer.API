namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.EthGeth
{
    public class EthBlockResponse
    {
        public string Hash { get; set; }
        public string Number { get; set; }
        public string? ParentHash { get; set; }
        public virtual ICollection<EthTransactionResponse> Transactions { get; set; }
    }
}
