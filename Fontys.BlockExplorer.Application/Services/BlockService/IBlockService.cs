namespace Fontys.BlockExplorer.Application.Services.BlockService
{
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;

    public interface IBlockService
    {
        public Task<Block?> GetBlockAsync(GetBlockCommand getBlockCommand);
    }
}
