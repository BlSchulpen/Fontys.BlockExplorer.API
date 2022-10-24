using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.Extensions.Logging;
using Moq;
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
        public async Task RequestGenesisBlock_BlockExcists_ReturnBlockSuccess()
        {
            // arrange
            var genesis_block = GenerateBlockResponse(0);
            var nodeService = new Mock<IBtcNodeService>();
            nodeService.Setup(x => x.GetBestBlockHashAsync()).ReturnsAsync(genesis_block.Hash);
            nodeService.Setup(x => x.GetHashFromHeightAsync(genesis_block.Height)).ReturnsAsync(genesis_block.Hash);
            nodeService.Setup(x => x.GetBlockFromHashAsync(genesis_block.Hash)).ReturnsAsync(genesis_block);

            var service = new BtcBlockProviderService(nodeService.Object,_mapper,_logger.Object);

            // act
            var result = service.GetBlockAsync(genesis_block.Hash);

            // assert
            result.Should().BeEquivalentTo(genesis_block);
        }

        public BtcBlockResponse GenerateBlockResponse(int nr)
        {
            var block = new BtcBlockResponse()
            {
                Hash = nr.ToString(),
                Height = nr,
                Previousblockhash = (nr-1).ToString(),
                Tx = new List<BtcBlockTxResponse>()

            };
            return block;
        }
    }
}
