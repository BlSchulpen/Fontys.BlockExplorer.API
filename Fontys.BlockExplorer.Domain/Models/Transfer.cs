namespace Fontys.BlockExplorer.Domain.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Transfer
    {
        [Key]
        public Guid Id { get; set; }    

        [Required]
        public long Value { get; set; }

        public Address Address { get; set; }    
    }
}
