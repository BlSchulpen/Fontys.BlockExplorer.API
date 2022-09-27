using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;

namespace Fontys.BlockExplorer.API.UnitTests.Factories
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
            return new BtcBlockResponse() {Hash = nrBlock.ToString(), Height = nrBlock, Transactions = transactions};
        }
    }
}
