namespace Fontys.BlockExplorer.NodeWarehouse.NodeServices
{
    using Fontys.BlockExplorer.Domain.Models;

    public interface INodeService
    {
        Task<string> GetBestBlockHashAsync();
        Task<string> GetHashFromHeight(int height);
        Task<Block> GetBlockFromHashAsync(string hash);
    }
}
