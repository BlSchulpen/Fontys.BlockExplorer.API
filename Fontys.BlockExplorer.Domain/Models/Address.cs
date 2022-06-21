namespace Fontys.BlockExplorer.Domain.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Address
    {
        [Key]
        public string Hash { get; set; }
    }
}
