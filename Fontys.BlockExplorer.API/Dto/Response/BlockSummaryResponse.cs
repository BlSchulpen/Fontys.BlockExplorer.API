using Fontys.BlockExplorer.Domain.Enums;

namespace Fontys.BlockExplorer.API.Dto.Response
{
    public class BlockSummaryResponse
    {
        public int Height { get; init; }

        public int NumberOfTransactions { get; init; }

        public DateTime CreationDateTime { get; init; }

        public CoinType CoinType { get; init; }
    }
}
