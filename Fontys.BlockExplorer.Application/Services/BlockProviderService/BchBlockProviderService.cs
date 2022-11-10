using AutoMapper;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Bch;
using Microsoft.Extensions.Logging;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class BchBlockProviderService : IBlockDataProviderService
    {
        private readonly IMapper _mapper;
        private readonly IBchNodeService _nodeService;
        private readonly ILogger<BchBlockProviderService> _logger;

        public BchBlockProviderService(IBchNodeService nodeService, ILogger<BchBlockProviderService> logger, IMapper mapper)
        { 
            _nodeService = nodeService;
            _logger = logger;
            _mapper = mapper;   
        }

        public async Task<Block> GetBlockAsync(string hash)
        {
            var response = await _nodeService.GetBlockFromHashAsync(hash);
            if (response == null)
            {
                _logger.LogError("Could not retrieve BCH block {Hash}", hash);
                throw new NullReferenceException();
            }
            var rawTransactions = await GetRawTransactionsAsync(response);
            var transactions = new List<Transaction>();
            rawTransactions.ForEach(r => transactions.Add(_mapper.Map<Transaction>(r)));
            var block = _mapper.Map<Block>(response);
            block.Transactions = transactions;
            return block;
        }

        public async Task<string> GetHashFromHeightAsync(int height)
        {
            var hash = await _nodeService.GetHashFromHeightAsync(height);
            if (hash == null)
            {
                _logger.LogError("Could not retrieve hash for BCH block with height: {height}", height); //TOOD you might want to log error but the error is thrown in the infrastructure layer in the bchNodeService
                throw new NullReferenceException();
            }
            return hash;
        }

        public async Task<string> GetBestBlockHashAsync()
        {
            var hash = await _nodeService.GetBestBlockHashAsync();
            if (hash == null)
            {
                _logger.LogError("Could not retrieve best BCH block"); //TOOD you might want to log error but the error is thrown in the infrastructure layer in the bchNodeService
                throw new NullReferenceException();
            }
            return hash;
        }

        private async Task<List<BchRawTransactionResponse>> GetRawTransactionsAsync(BchBlockResponse block)
        {
            var rawTransactionResponses = new List<BchRawTransactionResponse>();
            foreach (var transactionResponse in block.Tx)
            {
                var rawTransactionResponse = await _nodeService.GetRawTransactionAsync(transactionResponse.Hash);
                if (rawTransactionResponse == null)
                {
                    _logger.LogError("Could not retrieve raw for transaction with BCH hash: {Hash}", transactionResponse.Hash); //TOOD you might want to log error but the error is thrown in the infrastructure layer in the bchNodeService
                    throw new NullReferenceException();
                }
                rawTransactionResponses.Add(rawTransactionResponse);
            }
            return rawTransactionResponses;
        }
    }
}
