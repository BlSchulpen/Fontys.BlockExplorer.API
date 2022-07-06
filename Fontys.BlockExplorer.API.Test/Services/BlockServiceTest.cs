namespace Fontys.BlockExplorer.API.Test.Services
{
    using FluentAssertions;
    using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Domain.Models;
    using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
    using Moq;
    using Moq.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class BlockServiceTest
    {
        private readonly ExplorerMonitoringService _monitoringService;
        private readonly Mock<INodeService> _nodeServiceMock = new Mock<INodeService>();
        private readonly Mock<BlockExplorerContext> _dbContextMock = new Mock<BlockExplorerContext>();
        private readonly int _nrBlocks = 3;


        public BlockServiceTest()
        {
            _monitoringService = new ExplorerMonitoringService(_dbContextMock.Object, _nodeServiceMock.Object);
        }

        [Fact]
        public async Task Get_BadBlock_NoBad()
        {
            // arrange
            var blocks = MockBlocks();
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(blocks);
            SetupUpNodeService(blocks);

            // act
            await _monitoringService.RemoveBadBlocksAsync();

            // assert
            _dbContextMock.Object.Blocks.Should().HaveCount(_nrBlocks);
        }

        private void SetupUpNodeService(List<Block> blocks) {
            
            _nodeServiceMock.Setup(x => x.GetBestBlockHashAsync()).ReturnsAsync("1");
            foreach (var block in blocks)
            {
                _nodeServiceMock.Setup(x => x.GetBlockFromHashAsync(block.Hash)).ReturnsAsync(block);
                _nodeServiceMock.Setup(x => x.GetHashFromHeightAsync(block.Height)).ReturnsAsync(block.Hash);
            }
        }

        private List<Block> MockBlocks() 
        {
            var newBlocks = new List<Block>();
            for (int i = 0; i < _nrBlocks; i++)
            {
                newBlocks.Add(new Block() { Height = i, Hash = i.ToString()});
            }
            return newBlocks;
        }        
    }
}
