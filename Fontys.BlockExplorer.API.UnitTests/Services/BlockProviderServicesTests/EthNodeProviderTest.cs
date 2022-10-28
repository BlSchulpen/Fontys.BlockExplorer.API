using AutoMapper;
using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.EthGeth;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.BlockProviderServicesTests
{
    public class EthNodeProviderTest
    {
        private readonly Mock<ILogger<EthBlockProviderService>> _logger;
        private readonly IMapper _mapper;
        public EthNodeProviderTest()
        {
            _logger = new Mock<ILogger<EthBlockProviderService>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<EthProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task RequestGenesisBlock_BlockExists_ReturnBlockSuccessfully()
        {
            // arrange
            const int blockNr = 0;
            var expectedResponse = GenerateBlockResponse(blockNr);
            var mockNodeService = new Mock<IEthNodeService>();
            mockNodeService.Setup(x => x.GetBlockByHashAsync(blockNr.ToString())).ReturnsAsync(expectedResponse);

            //todo setup
            var blockProvider = new EthBlockProviderService(mockNodeService.Object,_mapper);

            // act
            var responseBlock = await blockProvider.GetBlockAsync(blockNr.ToString());

            // assert
            responseBlock.Should().BeEquivalentTo(responseBlock);
        }

        [Fact]
        public async Task RequestGenesisBlock_BlockNotExists_ThrowsNotFoundException()
        {
            // arrange
            const int blockNr = 0;
            var mockNodeService = new Mock<IEthNodeService>();

            //todo setup
            var blockProvider = new EthBlockProviderService(mockNodeService.Object, _mapper);

            // act
            Func<Task> f = async () => { await blockProvider.GetBlockAsync(blockNr.ToString()); };

            // assert
            await f.Should().ThrowAsync<DllNotFoundException>();
        }

        public EthBlockResponse GenerateBlockResponse(int nr)
        {
            var block = new EthBlockResponse()
            {
                Hash = nr.ToString(),
                Transactions = new List<EthTransactionResponse>()
            };
            return block;
        }

    }
}
