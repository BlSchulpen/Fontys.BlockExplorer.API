namespace Fontys.BlockExplorer.API.Test.Services
{
    using FluentAssertions;
    using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
    using Moq;
    using System.Threading.Tasks;
    using Xunit;

    public class BlockServiceTest
    {
        private readonly ExplorerMonitoringService _monitoringService;
        private readonly Mock<INodeService> _nodeServiceMock = new Mock<INodeService>();
        private readonly Mock<BlockExplorerContext> _dbContextMock = new Mock<BlockExplorerContext>();


        public BlockServiceTest()
        {
            _monitoringService = new ExplorerMonitoringService(_dbContextMock.Object, _nodeServiceMock.Object);
        }

        [Fact]
        public async Task Get_BadBlock_ReturnBad()
        {
            // arrange
           

            // act
            await _monitoringService.RemoveBadBlocksAsync();

            // assert
            _dbContextMock.Object.Blocks.Should().HaveCount(2);
        }
    }
}
