namespace Fontys.BlockExplorer.API.Dto.Response
{
    public class TransactionResponse
    {
        public string Hash { get; init; }

        public virtual ICollection<TransferResponse> Transfers { get; init; }
    }
}
