namespace Fontys.BlockExplorer.Domain.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        [Key]
        public string Hash { get; set; }

        public virtual ICollection<TxInput>? Inputs { get; set; }

        [Required]
        public virtual ICollection<TxOutput> Outputs { get; set; }
    }
}
