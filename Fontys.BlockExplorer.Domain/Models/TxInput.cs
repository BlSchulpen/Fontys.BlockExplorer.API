namespace Fontys.BlockExplorer.Domain.Models
{
    public class TxInput : Transfer
    {
        public bool IsNewlyGenerated { get; set; }
    }
}
