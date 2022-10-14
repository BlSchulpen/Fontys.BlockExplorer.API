using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
using System.Collections.Generic;

namespace Fontys.BlockExplorer.API.UnitTests.Factories.ResponseFactories
{
    public class BtcCoreBlockResponseFactory
    {
        public BtcBlockResponse BlockResponse(int nrBlock, int nrTransactions)
        {
            var transactions = new List<BtcBlockTxResponse>();
            for (var i = 0; i < nrTransactions; i++)
            {
                transactions.Add(new BtcBlockTxResponse()
                {
                    Hash = i.ToString()
                });
            }
            return new BtcBlockResponse() { Hash = nrBlock.ToString(), Height = nrBlock, Tx = transactions };
        }
    }
}
