using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.BlockService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class BlockServiceTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;
        private readonly Mock<IAddressRestoreService> _mockAddressRestoreService;
        private readonly Mock<ILogger<ExplorerBlockService>> _logger;

        public BlockServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _mockAddressRestoreService = new Mock<IAddressRestoreService>();
            _logger = new Mock<ILogger<ExplorerBlockService>>();
        }

        [Fact]
        public async Task GetBlock_BlockExists_ReturnStored()
        {
            // arrange
            const string blockHash = "0";
            var storedBlocks = new List<Block>
            {
                new() { Hash = blockHash }
            };
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);
            var service = new ExplorerBlockService(_dbContextMock.Object, _mockAddressRestoreService.Object, _logger.Object);

            //act
            var result = await service.GetBlockAsync(blockHash, CoinType.BTC);

            //assert 
            result?.Hash.Should().Be(blockHash);
        }

        [Fact]
        public async Task GetBlock_BlockNotExists_ReturnNull()
        {
            // arrange
            const string blockHash = "0";
            var storedBlocks = new List<Block>
            {
                new Block() { Hash = blockHash }
            };
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);
            var service = new ExplorerBlockService(_dbContextMock.Object, _mockAddressRestoreService.Object, _logger.Object);

            //act
            var result = await service.GetBlockAsync(blockHash, CoinType.BTC);

            //assert 
            result?.Hash.Should().Be(blockHash);
        }
    }
}
