namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore
{
    public class BtcOutputResponse
    {
        public double Value { get; set; }
        public int N { get; set; } //todo maybe consider somehow mapping this...
        public BtcScriptPubKeyResponse ScriptPubKey { get; set; }
    }
}
