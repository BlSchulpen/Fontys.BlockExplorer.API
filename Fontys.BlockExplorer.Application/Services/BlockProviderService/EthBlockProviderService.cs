using AutoMapper;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class EthBlockProviderService : IBlockDataProviderService
    {
        private readonly IMapper _mapper;
        private readonly IEthNodeService _ethNodeService;
        private readonly ILogger<EthBlockProviderService> _logger;

        //TODO error handling feedback
        public EthBlockProviderService(IEthNodeService ethNodeService, IMapper mapper, ILogger<EthBlockProviderService> logger)
        {
            _ethNodeService = ethNodeService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Block> GetBlockAsync(string hash)
        {
            var blockResponse = await _ethNodeService.GetBlockByHashAsync(hash);
            if (blockResponse == null)
            {
                _logger.LogError("Could not retrieve ETH block {Hash}", hash);
                throw new NullReferenceException();
            }
            var block = _mapper.Map<Block>(blockResponse);
            return block;
        }

        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var blockResponse = await _ethNodeService.GetBlockByNumberAsync(height);
            if (blockResponse == null)
            {
                _logger.LogError("Could not retrieve ETH block with height: {Height}", height);
            }
            return blockResponse.Hash;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            var latestNumber = await _ethNodeService.GetLatestNumber();
            var hash = await GetHashFromHeightAsync(latestNumber);
            return hash;
        }
    }
}
