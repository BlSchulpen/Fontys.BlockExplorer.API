namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore
{
    public class BtcOutputResponse
    {
        public long Value { get; set; }
        public int N { get; set; } //todo maybe consider somehow mapping this...
        public BtcAddressResponse Address { get; set; } 
    }
}
