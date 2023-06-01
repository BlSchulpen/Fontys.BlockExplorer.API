using AutoMapper;
using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.BlockProviderServicesTests
{
    public class BtcBlockProviderTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<BtcBlockProviderService>> _logger;

        public BtcBlockProviderTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<BtcProfile>());
            var mapper = config.CreateMapper();
            _mapper = mapper;
            _logger = new Mock<ILogger<BtcBlockProviderService>>();
        }


        [Fact]
        public async Task RequestGenesisBlock_BlockExists_ReturnBlockSuccess()
        {
            // arrange
            var genesisBlock = GenerateBlockResponse(0);
            var nodeService = new Mock<IBtcNodeService>();
            nodeService.Setup(x => x.GetBestBlockHashAsync()).ReturnsAsync(genesisBlock.Hash);
            nodeService.Setup(x => x.GetHashFromHeightAsync(genesisBlock.Height)).ReturnsAsync(genesisBlock.Hash);
            nodeService.Setup(x => x.GetBlockFromHashAsync(genesisBlock.Hash)).ReturnsAsync(genesisBlock);

            var service = new BtcBlockProviderService(nodeService.Object, _mapper, _logger.Object);

            // act
            var result = await service.GetBlockAsync(genesisBlock.Hash);

            // assert
            result.Should().BeEquivalentTo(genesisBlock);
        }

        [Fact]
        public async Task RequestGenesisBlock_BlockNotExists__NullRefrenceException()
        {
            // arrange
            var genesisBlock = GenerateBlockResponse(0);
            var nodeService = new Mock<IBtcNodeService>();
            nodeService.Setup(x => x.GetBestBlockHashAsync()).ReturnsAsync(genesisBlock.Hash);
            nodeService.Setup(x => x.GetHashFromHeightAsync(genesisBlock.Height)).ReturnsAsync(genesisBlock.Hash);
            nodeService.Setup(x => x.GetBlockFromHashAsync(genesisBlock.Hash)).ReturnsAsync(genesisBlock);

            var service = new BtcBlockProviderService(nodeService.Object, _mapper, _logger.Object);

            // act
            var result = await service.GetBlockAsync(genesisBlock.Hash);

            // assert
            result.Should().BeEquivalentTo(genesisBlock);
        }


        public BtcBlockResponse GenerateBlockResponse(int nr)
        {

            var block = new BtcBlockResponse()
            {
                Hash = nr.ToString(),
                Height = nr,
                Previousblockhash = nr > 0 ? (nr - 1).ToString() : null,
                Tx = new List<BtcBlockTxResponse>()
            };
            return block;
        }
    }
}
