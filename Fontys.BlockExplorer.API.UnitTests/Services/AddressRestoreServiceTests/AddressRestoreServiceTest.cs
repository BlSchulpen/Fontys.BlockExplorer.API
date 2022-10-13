using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fontys.BlockExplorer.API.UnitTests.Factories;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.AddressRestoreServiceTests
{
    public class AddressRestoreServiceTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;

        public AddressRestoreServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
        }

        [Fact]
        public async Task RestoreAddresses_NoneAreStored_OneNewAddress()
        {
            // arrange
            var newAddress = new Address { Hash = "0000" };
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(new List<Address>());
            var service = new ExplorerAddressRestoreService(_dbContextMock.Object);

            var inputs = MockInputTransfer(new List<Address>() { newAddress });
            var outputs = MockOutputTransfer(new List<Address>());
            var transaction = new Transaction() { Hash = "0000", Inputs = inputs, Outputs = outputs };
            var block = new Block() { Hash = "0000", CoinType = CoinType.BTC, Height = 0, NetworkType = NetworkType.BtcMainet, Transactions = new List<Transaction>() { transaction } };

            // act
            var newAddresses = await service.RestoreAddressesAsync(block);

            // assert
            newAddresses.Should().HaveCount(1);
        }

        [Fact]
        public async Task RestoreAddresses_NoneAreStored_NoNewAddresses()
        {
            // arrange
            var storedAddress = new Address() { Hash = "0000" };
            var newAddress = new Address { Hash = "0000" };
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(new List<Address>() { storedAddress });
            var service = new ExplorerAddressRestoreService(_dbContextMock.Object);

            var inputs = MockInputTransfer(new List<Address>() { newAddress });
            var outputs = MockOutputTransfer(new List<Address>());
            var transaction = new Transaction() { Hash = "0000", Inputs = inputs, Outputs = outputs };
            var block = new Block() { Hash = "0000", CoinType = CoinType.BTC, Height = 0, NetworkType = NetworkType.BtcMainet, Transactions = new List<Transaction>() { transaction } };

            // act
            var newAddresses = await service.RestoreAddressesAsync(block);

            // assert
            newAddresses.Should().HaveCount(0);
        }

        //todo maybe create a new object os you can easily pass all settings such as values and isGenerated
        private static List<TxInput> MockInputTransfer(List<Address> inputAddresses)
        {
            var inputTransfers = new List<TxInput>();
            inputAddresses.ForEach(address => inputTransfers.Add(new TxInput() { Address = address, Value = 0.12 }));
            return inputTransfers;
        }

        private static List<TxOutput> MockOutputTransfer(List<Address> outputAddresses)
        {
            var outputTransfer = new List<TxOutput>();
            outputAddresses.ForEach(address => outputTransfer.Add(new TxOutput() { Address = address, Value = 0.12 }));
            return outputTransfer;
        }

        [Fact]
        public void TestNrAddresses()
        {
            const int nrStoredAddresses = 100000;
            const int nrOldAddresses = 2000;
            const int nrNewAddresses = 7000;

            var storedAddresses = BlockFactory.StoredAddresses(nrStoredAddresses);
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(storedAddresses);
            var oldAddresses = BlockFactory.StoredAddresses(nrOldAddresses);
            var newAddresses = BlockFactory.NewAddresses(nrStoredAddresses, nrNewAddresses);
            var addresses = oldAddresses.Concat(newAddresses).ToList();
            var block = BlockFactory.NewBlock(addresses);

            var addressesInBlock = new List<Address>();
            var inputs = block.Transactions.SelectMany(t => t.Inputs).ToList();
            var outputs = block.Transactions.SelectMany(t => t.Outputs).ToList();
            inputs.ForEach(i => addressesInBlock.Add(i.Address));
            outputs.ForEach(i => addressesInBlock.Add(i.Address));

            var test = 0;
            var unique = addressesInBlock.Distinct().ToList();
            block.Should().NotBeNull();
        }
    }
}
