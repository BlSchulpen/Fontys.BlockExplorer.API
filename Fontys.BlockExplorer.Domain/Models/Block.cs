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

        [Required]
        public string PreviousHash { get; set; }

        [Required]
        public CoinType CoinType { get; set; }

        [Required]
        public NetworkType NetworkType { get; set; }

        [Required]
        public ICollection<Transaction> Transactions { get; set; }
    }
}
