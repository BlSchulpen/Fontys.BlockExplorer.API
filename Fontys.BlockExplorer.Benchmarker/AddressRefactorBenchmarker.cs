using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using Moq.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MyBenchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    public class AddressRefactorBenchmarker
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;

        public AddressRefactorBenchmarker()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
        }

        //17 seconds
        [Benchmark]
        public async Task RestoreAddresses()
        {
            // arrange
            int nrStoredAddresses = 100000;
            int nrOldAddresses = 2000;
            int nrNewAddresses = 10000;

            var storedAddresses = StoredAddresses(nrStoredAddresses);
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(storedAddresses);
            var oldAddresses = StoredAddresses(nrOldAddresses);
            var newAddresses = NewAddresses(nrStoredAddresses, nrNewAddresses);
            var addresses = oldAddresses.Concat(newAddresses).ToList();
            var block = NewBlock(addresses);
            var addressRestorer = new ExplorerAddressRestoreService(_dbContextMock.Object);

            //act
            await addressRestorer.RestoreAddressesAsync(block);
        }

        private List<Address> StoredAddresses(int nrAddresses)
        {
            var addresses = new List<Address>();
            for (int i = 0; i < nrAddresses; i++)
            {
                addresses.Add(new Address() { Hash = i.ToString() });
            }
            return addresses;
        }

        private List<Address> NewAddresses(int nrStoredAddresses, int nrNewAddresses)
        {
            var addresses = new List<Address>();
            for (int i = nrStoredAddresses; i < (nrStoredAddresses + nrNewAddresses); i++)
            {
                addresses.Add(new Address() { Hash = i.ToString() });
            }
            return addresses;
        }

        private Block NewBlock(List<Address> addresses)
        {
            var inputs = new List<TxInput>();
            foreach (var address in addresses)
            {
                inputs.Add(new TxInput() { Id = new Guid("3f78316e-3ff9-46c7-ae2d-660f4516c2c6"), Address = address, IsNewlyGenerated = false, Value = 0 });
            }
            var transaction = new Transaction() { Hash = "0", Inputs = inputs, Outputs = new List<TxOutput>() };
            var block = new Block() { Hash = "0", CoinType = CoinType.BTC, NetworkType = NetworkType.BtcMainet, Height = 0, PreviousBlockHash = "0", Transactions = new List<Transaction>() { transaction } };
            return block;
        }
    }
}