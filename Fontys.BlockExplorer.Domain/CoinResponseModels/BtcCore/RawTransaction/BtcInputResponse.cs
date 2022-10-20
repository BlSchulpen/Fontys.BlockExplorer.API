namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.RawTransaction
{
    public class BtcInputResponse
    {
        public string TxId { get; set; }
        public int Vout { get; set; }
        public string? Coinbase { get; set; }
        public double? Value { get; set; }
        public List<string>? Addresses { get; set; }
    }
}
