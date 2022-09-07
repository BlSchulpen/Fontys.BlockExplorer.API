using Fontys.BlockExplorer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc
{
    public class Destination
    {
        [Key]
        public string Hash { get; set; }

        [Required]
        public int Height { get; set; }

        public string? PreviousHash { get; set; }
        
        [Required]
        public CoinType CoinType { get; set; }

        [Required]
        public NetworkType NetworkType { get; set; }
        
        [Required]
        public virtual ICollection<DestinationAddition> Transactions { get; set; }
        
    }
}
