namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.RawTransaction
{
    public class BtcOutputResponse
    {
        public double Value { get; set; }
        public int N { get; set; }
        public BtcScriptPubKeyResponse ScriptPubKey { get; set; }
    }
}
