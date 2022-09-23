using BenchmarkDotNet.Configs;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Xunit;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
using Moq;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class BtcBlockProviderTest
    {
        [Fact]
        public async Task GetBlock_BlockExists_ReturnBlock()
        {
            //arrange
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            const string hash = "0000";
            var blockInChain = new Block() { Hash = hash };
            var mockNodeService = new Mock<IBtcNodeService>();

            var service = new BtcBlockProviderService(mockNodeService.Object,mapper);

            //act
            var result = service.GetBlockAsync(hash);

            //assert
            mapper.Should().NotBeNull();
        }

        private Mock<IBtcNodeService> GetMockNodeService(string blockHash)
        {
            var mockNodeService = new Mock<IBtcNodeService>();
            var btcBlockResponse = new BtcBlockResponse() { Hash = blockHash, Height = 0, Previousblockhash = "9999",  };
            mockNodeService.Setup(nodeService => nodeService.GetBlockFromHashAsync(blockHash).Result());
            return new Mock<IBtcNodeService>();

        }
    }
}
