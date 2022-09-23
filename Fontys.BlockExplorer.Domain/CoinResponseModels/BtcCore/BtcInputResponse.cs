namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore
{
    public class BtcInputResponse
    {
        public string TxId { get; set; }
        public int Vout { get; set; }
        public string? Coinbase { get; set; }

        //========================================
        //TODO should be removed from response --> never expect Value and addres has to be set manually, consider adding a new DTO
        public double? Value { get; set; }
        public List<string>? Addresses { get; set; }

    }
}
