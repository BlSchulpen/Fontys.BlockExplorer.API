namespace Fontys.BlockExplorer.Domain.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Block
    {
        [Key]
        public string Hash { get; set; }
        
        [Required]
        public int Height { get; set; }

        [Required]
        public string PreviousHash { get; set; }


        public ICollection<Transaction> transactions { get; set; }
    }
}
