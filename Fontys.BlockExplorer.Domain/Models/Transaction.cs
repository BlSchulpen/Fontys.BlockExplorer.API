namespace Fontys.BlockExplorer.Domain.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        [Key]
        public string Hash { get; set; }

        public ICollection<Transfer> Transfers { get; set; }    

    }
}
