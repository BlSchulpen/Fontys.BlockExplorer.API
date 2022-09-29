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
using Fontys.BlockExplorer.Application.Services.BlockService;
using Moq.EntityFrameworkCore;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class BlockServiceTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;

        public BlockServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
        }

        [Fact]
        public async Task GetBlock_BlockExists_ReturnStored()
        {
            // arrange
            const string blockHash = "0";
            var storedBlocks = new List<Block>
            {
                new Block() { Hash = blockHash }
            };
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);
            var service = new ExplorerBlockService(_dbContextMock.Object);
            var blockCommand = new GetBlockCommand() {Hash = blockHash};
            
            //act
            var result = await service.GetBlockAsync(blockCommand);

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
            var service = new ExplorerBlockService(_dbContextMock.Object);
            var blockCommand = new GetBlockCommand() { Hash = blockHash };

            //act
            var result = await service.GetBlockAsync(blockCommand);

            //assert 
            result?.Hash.Should().Be(blockHash);
        }
    }
}
