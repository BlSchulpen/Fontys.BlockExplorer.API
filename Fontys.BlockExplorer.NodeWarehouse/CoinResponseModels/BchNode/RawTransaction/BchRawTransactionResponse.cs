using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction
{
    public class BchRawTransactionResponse
    {
        public string TxId { get; set; }
        public List<BchVinResponse> Vin { get; set; }
        public List<BchVoutResponse> Vout { get; set; }

    }
}
