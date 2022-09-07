using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    public class Source
    {
        public string Hash { get; set; }
        public int Height { get; set; }
        public string? Previousblockhash { get; set; }
        public virtual ICollection<SourceAddition> Tx { get; set; }
    }
}
