using AutoMapper;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class BtcBlockProviderService : IBlockDataProviderService
    {
        private readonly IBtcNodeService _btcNodeService;
        private readonly IMapper _mapper;

        public BtcBlockProviderService(IBtcNodeService btcNodeService, IMapper mapper)
        {
            _btcNodeService = btcNodeService;
            _mapper = mapper;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            var hash = await _btcNodeService.GetBestBlockHashAsync();
            return hash;
        }
        public async Task<string> GetHashFromHeightAsync(int height)
        {
            try
            {
                var hash = await _btcNodeService.GetHashFromHeightAsync(height);
                return hash;
            }
            catch (NullReferenceException e)
            {
                //TODO add logger
                throw;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task<Block> GetBlockAsync(string hash)
        {
            var blockResponse = await _btcNodeService.GetBlockFromHashAsync(hash);
            var transactionIds = blockResponse.Tx.Select(t => t.hash);
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
            foreach (var input in rawTransaction.Vin.Where(t => t.TxId != null)) //todo check if it could be null
            {
                var usedOutput = rawTransaction.Vout.FirstOrDefault(v => v.N == input.Vout); //inputs of this transaction are the outputs of another transaction
                if (usedOutput == null)
                {
                    continue;
                }
                input.Addresses = usedOutput.ScriptPubKey.Addresses; //maybe try ti get this info from key instead to improve performance...
                input.Value = usedOutput.Value;
            }
        }
    }
}
