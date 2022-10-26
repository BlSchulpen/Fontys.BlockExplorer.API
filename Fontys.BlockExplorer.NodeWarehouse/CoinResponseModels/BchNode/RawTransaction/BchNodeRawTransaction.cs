using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction
{
    public class BchNodeRawTransaction
    {
        public string TxId { get; set; }
        public List<BchNodeVin> Vin { get; set; }
        public List<BchNodeVout> Vout { get; set; }

    }
}
