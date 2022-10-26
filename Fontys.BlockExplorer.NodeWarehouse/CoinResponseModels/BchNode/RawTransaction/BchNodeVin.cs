using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction
{
    public class BchNodeVin
    {
        public int Vout { get; set; }
        public BchNodeScriptSig ScriptSig { get; set; }
    }
}
