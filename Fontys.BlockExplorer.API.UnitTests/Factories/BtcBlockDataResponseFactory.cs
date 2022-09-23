using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.API.UnitTests.Factories
{
    public class BtcBlockDataResponseFactory : IBlockResponseDataFactory
    {
        public BtcBlockResponse CreateBlockResponse(string hash, int height, string previousHash, int nrTransactions )
        {
            var transactions = new List<BtcTransactionResponse>();
            for (int i = 0; i < nrTransactions; i++)
            {

            }

            var block = new BtcBlockResponse(){ Hash = hash, Height = height, Previousblockhash = previousHash};
            return null;
        }

        public BtcTransactionResponse CreateTransactionResponse(string hash, List<BtcInputResponse> vin, List<BtcOutputResponse> vout)
        {
            var transaction = new BtcTransactionResponse()
            {
                Hash = hash,
                Vin = vin,
                Vout = vout
            };
            return transaction;
        }

        public BtcInputResponse CreateInputResponse(string txId, List<string> addresses, int vout, string coinbase)
        {
            var input = new BtcInputResponse()
            {
                TxId = txId,
                Vout = vout,
                Coinbase = coinbase,
                Addresses = addresses,
            };
            return input;
        }

        public BtcOutputResponse CreateOutputResponse(int n, int value, List<string> addresses)
        {
            var output = new BtcOutputResponse()
            {
                N = n, 
                Value = value, 
                ScriptPubKey = new BtcScriptPubKeyResponse() {Addresses = addresses}
            };
            return output;
        }

        public BtcAddressResponse CreateAddressResponse(string address)
        {
            return new BtcAddressResponse() { Hash = address};
        }
    }
}
