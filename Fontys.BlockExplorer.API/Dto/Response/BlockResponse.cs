namespace Fontys.BlockExplorer.API.Dto.Response
{
    public class BlockResponse
    {
        public string Hash { get; init; }

        public int Height { get; init; }

        public string? PreviousBlockHash { get; init; }

        public virtual ICollection<TransactionResponse> Transactions { get; init; }
    }
}
