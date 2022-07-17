namespace Fontys.BlockExplorer.Domain.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        [Key]
        public string Hash { get; set; }

        public virtual ICollection<Transfer> Transfers { get; set; }    

    }
}
