namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    using AutoMapper;
    using FluentAssertions;
    using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Domain.Models;
    using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;
    using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
    using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
    using Moq;
    using Moq.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class BlockServiceTest
    {
        private readonly ExplorerMonitoringService _monitoringService;
        private readonly Mock<INodeService> _nodeServiceMock;
        private readonly Mock<BlockExplorerContext> _dbContextMock;
        private readonly int _nrBlocks = 3;

        public BlockServiceTest()
        {
            _nodeServiceMock = new Mock<INodeService>();
            _dbContextMock = new Mock<BlockExplorerContext>();
            var profile = new BtcProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            _monitoringService = new ExplorerMonitoringService(_dbContextMock.Object, _nodeServiceMock.Object, mapper);
        }

        [Fact]
        public async Task Get_BadBlock_ReturnEmpty()
        {
            // arrange
            var blocks = MockBlocks();
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(blocks);
            SetupUpNodeService();

            // act
            var removedBlocks = await _monitoringService.RemoveBadBlocksAsync();

            // assert
            removedBlocks.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_BadBlock_ReturnOne()
        {
            // arrange
            var blocks = MockBlocks();
            SetupUpNodeService();
            var wrongBlocks = new List<Block>(blocks);
            var removeBlock = blocks.LastOrDefault();
            wrongBlocks.Remove(removeBlock);
            wrongBlocks.Add(new Block() { Height = removeBlock.Height, Hash = "Wrong" });
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(wrongBlocks);

            // act
            var removedBlocks = await _monitoringService.RemoveBadBlocksAsync();

            // assert
            removedBlocks.Should().HaveCount(1);
        }

        [Fact]
        public async Task Get_NewBlocks_ReturnTwo()
        {
            // arrange
            var blocks = MockBlocks();
            SetupUpNodeService();
            var storedBlocks = new List<Block>() { blocks[0] };
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);

            // act
            var newBlocks = await _monitoringService.GetNewBlocksAsync();

            // assert
            newBlocks.Should().HaveCount(2);
        }

        [Fact]
        public async Task Get_NewBlocks_ReturnOne()
        {
            // arrange
            var blocks = MockBlocks();
            SetupUpNodeService();
            var storedBlocks = new List<Block>();
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);

            // act
            var newBlocks = await _monitoringService.GetNewBlocksAsync();

            // assert
            newBlocks.Should().HaveCount(3);
        }

        private void SetupUpNodeService()
        {
            var blocks = MockBlocksResponses();

            _nodeServiceMock.Setup(x => x.GetBestBlockHashAsync()).ReturnsAsync((_nrBlocks - 1).ToString());
            foreach (var block in blocks)
            {
                _nodeServiceMock.Setup(x => x.GetBlockFromHashAsync(block.Hash)).ReturnsAsync(block);
                _nodeServiceMock.Setup(x => x.GetHashFromHeightAsync(block.Height)).ReturnsAsync(block.Hash);
            }
        }

        private List<BtcBlockResponse> MockBlocksResponses()
        {
            var newBlocks = new List<BtcBlockResponse>();
            for (int i = 0; i < _nrBlocks; i++)
            {
                newBlocks.Add(new BtcBlockResponse() { Height = i, Hash = i.ToString() });
            }
            return newBlocks;
        }

        private List<Block> MockBlocks()
        {
            var newBlocks = new List<Block>();
            for (int i = 0; i < _nrBlocks; i++)
            {
                newBlocks.Add(new Block() { Height = i, Hash = i.ToString() });
            }
            return newBlocks;
        }
    }
}
