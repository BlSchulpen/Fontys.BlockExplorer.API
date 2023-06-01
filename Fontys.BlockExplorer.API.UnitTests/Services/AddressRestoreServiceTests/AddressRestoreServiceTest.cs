using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fontys.BlockExplorer.Application.Services.AddressService;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.AddressRestoreServiceTests
{
    public class AddressRestoreServiceTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;
        private readonly Mock<IAddressService> _mockAddressService;

        public AddressRestoreServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _mockAddressService = new Mock<IAddressService>();
        }
        /*
        [Fact]
        public async Task RestoreAddresses_NoneAreStored_OneNewAddress()
        {
            // arrange
            var newAddress = new Address { Hash = "0000" };
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(new List<Address>());
            var service = new ExplorerAddressRestoreService(_dbContextMock.Object, _mockAddressService.Object);

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
        public async Task RestoreAddresses_SeveralStored_AddressesConflict()
        {
            // arrange
            var newAddress = new Address { Hash = "0001" };
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(new List<Address>());
            var service = new ExplorerAddressRestoreService(_dbContextMock.Object, _mockAddressService.Object);

            var inputs = MockInputTransfer(new List<Address>() { newAddress });
            var outputs = MockOutputTransfer(new List<Address>());
            var transaction = new Transaction() { Hash = "0000", Inputs = inputs, Outputs = outputs };
            var second_transaction = new Transaction() { Hash = "0001", Inputs = inputs, Outputs = outputs };
            var block = new Block() { Hash = "0000", CoinType = CoinType.BTC, Height = 0, NetworkType = NetworkType.BtcMainet, Transactions = new List<Transaction>() { transaction, second_transaction } };

            // act
            var newAddresses = await service.RestoreAddressesAsync(block);

            // assert 
            newAddresses.Should().HaveCount(0);
        }



        [Fact]
        public async Task RestoreAddresses_NoneAreStored_NoNewAddresses()
        {
            // arrange
            var storedAddress = new Address() { Hash = "0000" };
            var newAddress = new Address { Hash = "0000" };
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(new List<Address>() { storedAddress });
            var service = new ExplorerAddressRestoreService(_dbContextMock.Object, _mockAddressService.Object);

            var inputs = MockInputTransfer(new List<Address>() { newAddress });
            var outputs = MockOutputTransfer(new List<Address>());
            var transaction = new Transaction() { Hash = "0000", Inputs = inputs, Outputs = outputs };
            var block = new Block() { Hash = "0000", CoinType = CoinType.BTC, Height = 0, NetworkType = NetworkType.BtcMainet, Transactions = new List<Transaction>() { transaction } };

            // act
            var newAddresses = await service.RestoreAddressesAsync(block);

            // assert
            newAddresses.Should().HaveCount(0);
        }

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
        */
    }
}
