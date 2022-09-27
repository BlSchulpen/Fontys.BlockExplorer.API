namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block
{
    public class BtcTransactionResponse
    {
        public string Hash { get; set; }
        public virtual ICollection<BtcInputResponse>? Vin { get; set; }
        public virtual ICollection<BtcOutputResponse>? Vout { get; set; }

    }
}
