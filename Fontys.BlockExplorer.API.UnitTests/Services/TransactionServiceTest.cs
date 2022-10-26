using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.TxService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;

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
            var transaction = new Transaction() { Hash = hash };
            var mockLogger = new Mock<ILogger<ExplorerTxService>>();
            _dbContextMock.Setup(x => x.Transactions).ReturnsDbSet(new List<Transaction> { transaction });
            var service = new ExplorerTxService(_dbContextMock.Object, mockLogger.Object);

            // act
            var txResult = await service.GetTransactionAsync(hash);

            // assert
            txResult.Should().BeEquivalentTo(transaction);
        }

        [Fact]
        public async Task GetTransaction_TransactionExists_ReturnNull()
        {
            // arrange
            const string hash = "1";
            var mockLogger = new Mock<ILogger<ExplorerTxService>>();
            _dbContextMock.Setup(x => x.Transactions).ReturnsDbSet(new List<Transaction>());
            var service = new ExplorerTxService(_dbContextMock.Object, mockLogger.Object);

            // act
            var txResult = await service.GetTransactionAsync(hash);

            // assert
            txResult.Should().BeNull();
        }
    }
}
