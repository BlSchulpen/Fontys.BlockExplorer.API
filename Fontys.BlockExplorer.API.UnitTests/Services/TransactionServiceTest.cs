using Fontys.BlockExplorer.Application.Services.AddressService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq.EntityFrameworkCore;
using Xunit;
using Fontys.BlockExplorer.Application.Services.TxService;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class TransactionServiceTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;

        public TransactionServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
        }

        [Fact]
        public async Task GetTransaction_TransactionExists_ReturnStored()
        {
            // arrange
            const string hash = "1";
            var transaction = new Transaction() { Hash = hash};
            var transactionCommand = new GetTxCommand() { Hash = hash };
            _dbContextMock.Setup(x => x.Transactions).ReturnsDbSet(new List<Transaction> { transaction });
            var service = new ExplorerTxService(_dbContextMock.Object);

            // act
            var txResult = await service.GetTransactionAsync(transactionCommand);

            // assert
            txResult.Should().BeEquivalentTo(transaction);
        }

        //TODO maybe it is better to return exception ==> services do exception handling
        [Fact]
        public async Task GetTransaction_TransactionExists_ReturnNull()
        {
            // arrange
            const string hash = "1";
            var transactionCommand = new GetTxCommand() { Hash = hash };
            _dbContextMock.Setup(x => x.Transactions).ReturnsDbSet(new List<Transaction>());
            var service = new ExplorerTxService(_dbContextMock.Object);

            // act
            var txResult = await service.GetTransactionAsync(transactionCommand);

            // assert
            txResult.Should().BeNull();
        }
    }
}
