using AutoMapper;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;

namespace Fontys.BlockExplorer.Application.Services.BlockProviderService
{
    public class BtcBlockProviderService : IBlockProviderService
    {
        private readonly IBtcNodeService _nodeService;
        private readonly IMapper _mapper;

        public BtcBlockProviderService(IBtcNodeService nodeService, IMapper mapper)
        {
            _nodeService = nodeService;
            _mapper = mapper;
        }

        public async Task<Block> GetBlockAsync(string hash)
        {
            var blockResponse = await _nodeService.GetBlockFromHashAsync(hash);
            foreach (var transaction in blockResponse.Tx)
            {
                var usedIndexes = new List<int>();
                foreach (var input in transaction.Vin.Where(t => t.TxId != null))
                {
                    var rawTransaction = await _nodeService.GetRawTransactionAsync(input.TxId);
                    var usedOutput = rawTransaction.Vout.FirstOrDefault(v => v.N == input.Vout); //inputs of this transaction are the outputs of another transaction
                    if (usedOutput != null)
                    {
                        input.Addresses = usedOutput.ScriptPubKey.Addresses; //maybe try ti get this info from key instead to improve preformance...
                        input.Value = usedOutput.Value;
                    }
                }
            }
            var block = _mapper.Map<Block>(blockResponse);
            return block;
        }
    }
}
