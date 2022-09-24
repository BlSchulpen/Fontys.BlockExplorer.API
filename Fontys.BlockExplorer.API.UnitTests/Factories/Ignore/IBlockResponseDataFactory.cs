using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
using System.Collections.Generic;

namespace Fontys.BlockExplorer.API.UnitTests.Factories.Ignore
{
    public interface IBlockResponseDataFactory
    {
        public BtcBlockResponse CreateBlockResponse(string hash, int height, string previousHash, int nrTransactions);

        public BtcTransactionResponse CreateTransactionResponse(string hash,
            List<BtcInputResponse> vin,
            List<BtcOutputResponse> vout);

        public BtcInputResponse CreateInputResponse(string txId, List<string> addresses, int vout, string coinbase);
        public BtcOutputResponse CreateOutputResponse(int n, int value, List<string> addresses);
        public BtcAddressResponse CreateAddressResponse(string address);
    }
}
