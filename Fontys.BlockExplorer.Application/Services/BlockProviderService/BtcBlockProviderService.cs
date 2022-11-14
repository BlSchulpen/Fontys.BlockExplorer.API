using AutoMapper;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class BtcBlockProviderService : IBlockDataProviderService
    {
        private readonly IBtcNodeService _btcNodeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BtcBlockProviderService(IBtcNodeService btcNodeService, IMapper mapper, ILogger<BtcBlockProviderService> logger)
        {
            _btcNodeService = btcNodeService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            var hash = await _btcNodeService.GetBestBlockHashAsync();

            return hash;
        }
        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var hash = await _btcNodeService.GetHashFromHeightAsync(height);
            return hash;
        }

        public async Task<Block> GetBlockAsync(string hash)
        {
            var blockResponse = await _btcNodeService.GetBlockFromHashAsync(hash);
            var transactionIds = blockResponse.Tx.Select(t => t.Hash);
            foreach (var transactionId in transactionIds)
            {
                await RetrieveNonCoinBasedInputDataAsync(transactionId);
            }
            var block = _mapper.Map<Block>(blockResponse);
            return block;
        }

        private async Task RetrieveNonCoinBasedInputDataAsync(string transactionId)
        {
            var rawTransaction = await _btcNodeService.GetRawTransactionAsync(transactionId);
            foreach (var input in rawTransaction.Vin.Where(t => t.TxId != null))
            {
                var usedOutput = rawTransaction.Vout.FirstOrDefault(v => v.N == input.Vout);
                if (usedOutput == null)
                {
                    continue;
                }
                input.Addresses = usedOutput.ScriptPubKey.Addresses;
                input.Value = usedOutput.Value;
            }
        }
    }
}
