namespace Fontys.BlockExplorer.Domain.Models
{
    using Fontys.BlockExplorer.Domain.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Block
    {
        [Key]
        public string Hash { get; set; }


        [Required]
        public int Height { get; set; }

        public string? Previousblockhash { get; set; }

        [Required]
        public CoinType CoinType { get; set; }

        [Required]
        public NetworkType NetworkType { get; set; }

       [Required]
       public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
