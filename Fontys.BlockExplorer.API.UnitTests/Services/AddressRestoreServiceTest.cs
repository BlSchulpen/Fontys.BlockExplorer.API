using AutoMapper;
using FluentAssertions;
using Fontys.BlockExplorer.API.Controllers;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.AddressService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Reflection.Metadata.BlobBuilder;

namespace Fontys.BlockExplorer.API.UnitTests.Services
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
            var service = new AddressRestoreService(_dbContextMock.Object);

            var inputs = MockInputTransfer(new List<Address>() { newAddress });
            var outputs = MockOutputTransfer(new List<Address>());
            var transaction = new Transaction() { Hash = "0000", Inputs = inputs, Outputs = outputs };
            var block = new Block() { Hash = "0000", CoinType = CoinType.BTC, Height = 0, NetworkType = NetworkType.BtcMainet, Transactions = new List<Transaction>() { transaction } };

            // act
            var newAddresses = await service.RestoreAddressesAsync(block);
            var test = _dbContextMock.Object.Addresses.ToList();

            // assert
            newAddresses.Should().HaveCount(1);
        }

        [Fact]
        public async Task RestoreAddresses_NoneAreStored_NoNewAddresess()
        {
            // arrange
            var storedAddress = new Address() { Hash = "0000" };
            var newAddress = new Address { Hash = "0000" };
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(new List<Address>() {storedAddress});
            var service = new AddressRestoreService(_dbContextMock.Object);

            var inputs = MockInputTransfer(new List<Address>() { newAddress });
            var outputs = MockOutputTransfer(new List<Address>());
            var transaction = new Transaction() { Hash = "0000", Inputs = inputs, Outputs = outputs };
            var block = new Block() { Hash = "0000", CoinType = CoinType.BTC, Height = 0, NetworkType = NetworkType.BtcMainet, Transactions = new List<Transaction>() { transaction } };

            // act
            var newAddresses = await service.RestoreAddressesAsync(block);
            var test = _dbContextMock.Object.Addresses.ToList();

            // assert
            newAddresses.Should().HaveCount(0);
        }



        //todo maybe create a new object os you can easily pass all settings suych as values and isGenerated
        private List<TxInput> MockInputTransfer(List<Address> inputAddresses)
        {
            var inputTransfers = new List<TxInput>();
            foreach (var inputAddress in inputAddresses)
            {
                inputTransfers.Add(new TxInput() { Address = inputAddress, IsNewlyGenerated = true, Value = 0.12 });
            }
            return inputTransfers;

        }

        private List<TxOutput> MockOutputTransfer(List<Address> outputAddresses)
        {
            var outputTransfer = new List<TxOutput>();
            foreach (var outputAddress in outputAddresses)
            {
                outputTransfer.Add(new TxOutput() { Address = outputAddress, Value = 0.12 });
            }
            return outputTransfer;
        }

    }
}
