namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block
{
    public class BtcOutputResponse
    {
        public double Value { get; set; }
        public int N { get; set; }
        public BtcScriptPubKeyResponse ScriptPubKey { get; set; }
    }
}
