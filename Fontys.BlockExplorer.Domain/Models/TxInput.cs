using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.Domain.Models
{
    public class TxInput : Transfer
    {
        public bool IsNewlyGenerated { get; set; }
    }
}
