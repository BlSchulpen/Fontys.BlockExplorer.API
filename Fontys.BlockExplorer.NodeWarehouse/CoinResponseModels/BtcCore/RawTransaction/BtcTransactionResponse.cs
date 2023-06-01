namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.RawTransaction
{
    public class BtcTransactionResponse
    {
        public string Hash { get; set; }
        public virtual ICollection<BtcInputResponse>? Vin { get; set; }
        public virtual ICollection<BtcOutputResponse>? Vout { get; set; }

    }
}
