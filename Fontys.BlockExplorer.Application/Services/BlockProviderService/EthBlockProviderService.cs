using AutoMapper;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class EthBlockProviderService : IBlockDataProviderService
    {
        private readonly IMapper _mapper;
        private readonly IEthNodeService _ethNodeService;

        public EthBlockProviderService(IEthNodeService ethNodeService, IMapper mapper)
        {
            _ethNodeService = ethNodeService;
            _mapper = mapper;
        }

        public async Task<Block> GetBlockAsync(string hash)
        {
            var blockResponse = await _ethNodeService.GetBlockByHashAsync(hash);
            var block = _mapper.Map<Block>(blockResponse);
            return block;
        }
        //TODO Eth can get block directly?
        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var blockResponse = await _ethNodeService.GetBlockByNumberAsync(height);
            return blockResponse.Hash;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            var latestNumber = await _ethNodeService.GetLatestNumber();
            var blockResponse = await _ethNodeService.GetBlockByNumberAsync(latestNumber);
            return blockResponse.Hash; 
        }
    }
}
