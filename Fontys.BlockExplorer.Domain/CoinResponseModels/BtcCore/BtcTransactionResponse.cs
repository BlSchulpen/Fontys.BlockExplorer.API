using Fontys.BlockExplorer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore
{
    public class BtcTransactionResponse
    {
        public string Hash { get; set; }
        public virtual ICollection<BtcInputResponse> Vin { get; set; }
        public virtual ICollection<BtcOutputResponse> Vout { get; set; }

    }
}
