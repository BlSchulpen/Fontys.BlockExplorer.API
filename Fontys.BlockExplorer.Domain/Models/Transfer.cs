namespace Fontys.BlockExplorer.Domain.Models
{
    using Fontys.BlockExplorer.Domain.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Transfer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public double Value { get; set; } //consder saving value as a long

//       [Required]
        public virtual Address? Address { get; set; }   //todo change to required
    }
}
