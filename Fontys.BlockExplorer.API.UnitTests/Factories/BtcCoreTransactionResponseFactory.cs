using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;

namespace Fontys.BlockExplorer.API.UnitTests.Factories
{
    public class BtcCoreTransactionResponseFactory
    {
        //TODO maybe parse a settings file --> with address info such as null, unique etc...
        public BtcTransactionResponse BtcTransactionResponse(int nrCoinBaseVin, int nrVout, int TxNummer)
        {
            var txId = TxNummer.ToString();
            var inputs = TransactionInputs(nrCoinBaseVin, txId);
            var outputs = TransactionOutputs(nrVout, txId);
            var transaction = new BtcTransactionResponse()
            {
                Hash = txId,
                Vin = inputs,
                Vout = outputs
            };
            return transaction;
        }

        private List<BtcInputResponse> TransactionInputs(int nrInputs, string txId)
        {
            var inputs = new List<BtcInputResponse>();
            for (var i = 0; i < nrInputs; i++)
            {
                inputs.Add(new BtcInputResponse()
                {
                    TxId = txId,
                    Coinbase = i.ToString(),
                    Addresses = new List<string>() { nrInputs.ToString()},
                });
            }
            return inputs;
        }

        private List<BtcOutputResponse> TransactionOutputs(int nrVout, string txId)
        {
            var outputs = new List<BtcOutputResponse>();
            for (var i = 0; i < nrVout; i++)
            {
                outputs.Add(new BtcOutputResponse()
                {
                    Value = 500, //TODO add settings file
                    N = i,
                    ScriptPubKey = new BtcScriptPubKeyResponse() { Addresses = new List<string>() {i.ToString()} },
                });
            }
            return outputs;
        }
    }
}
