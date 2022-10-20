using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using System;
using System.Collections.Generic;

namespace Fontys.BlockExplorer.API.UnitTests.Factories
{
    public static class BlockFactory
    {
        public static List<Address> StoredAddresses(int nrAddresses)
        {
            var addresses = new List<Address>();
            for (var i = 0; i < nrAddresses; i++)
            {
                addresses.Add(new Address() { Hash = i.ToString() });
            }
            return addresses;
        }

        public static List<Address> NewAddresses(int nrStoredAddresses, int nrNewAddresses)
        {
            var addresses = new List<Address>();
            for (var i = nrStoredAddresses; i < nrStoredAddresses + nrNewAddresses; i++)
            {
                addresses.Add(new Address() { Hash = i.ToString() });
            }
            return addresses;
        }

        public static Block NewBlock(List<Address> addresses)
        {
            var nonPickedAddresses = new List<Address>(addresses);
            const int avgNrTransactions = 2000;
            var transactions = new List<Transaction>();

            for (var i = 0; i < avgNrTransactions; i++)
            {
                var transaction = new Transaction() { Hash = i.ToString(), Inputs = GetAddressInputs(addresses, nonPickedAddresses), Outputs = new List<TxOutput>() };
                transactions.Add(transaction);
            }
            var block = new Block() { Hash = "0", CoinType = CoinType.BTC, NetworkType = NetworkType.BtcMainet, Height = 0, PreviousBlockHash = "0", Transactions = new List<Transaction>(transactions) };
            return block;
        }

        private static List<TxInput> GetAddressInputs(List<Address> addresses, List<Address> nonPickedAddresses)
        {
            var inputs = new List<TxInput>();
            var possibilities = new List<Address>(addresses);
            var random = new Random();
            int nrInputs = random.Next(1, 5);
            for (var i = 0; i < nrInputs; i++)
            {
                Address address;
                if (nonPickedAddresses.Count != 0)
                {
                    var index = random.Next(nonPickedAddresses.Count);
                    address = nonPickedAddresses[index];
                }
                else
                {
                    var index = random.Next(addresses.Count);
                    address = possibilities[index];
                }

                possibilities.Remove(address);
                inputs.Add(new TxInput() { Id = new Guid("3f78316e-3ff9-46c7-ae2d-660f4516c2c6"), Address = address, IsNewlyGenerated = false, Value = 10 });
            }
            return inputs;
        }
    }
}
