using Autofac.Core;
using AutoMapper;
using BenchmarkDotNet.Configs;
using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Bch;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Diagnostics.Tracing.StackSources;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.BlockProviderServicesTests
{
    public class BchBlockProviderTest
    {
        private readonly Mock<ILogger<BchBlockProviderService>> _logger;
        private readonly IMapper _mapper;
        public BchBlockProviderTest()
        {
            _logger = new Mock<ILogger<BchBlockProviderService>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<BchProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task RequestGenesisBlock_BlockExists_ReturnBlockSuccessfully() 
        {
            // arrange
            const int blockNr = 0;
            var expectedResponse = GenerateBlockResponse(blockNr);
            var mockNodeService = new Mock<IBchNodeService>();
            mockNodeService.Setup(x => x.GetBlockFromHashAsync(blockNr.ToString())).ReturnsAsync(expectedResponse);

            //todo setup
            var blockProvider = new BchBlockProviderService(mockNodeService.Object,_logger.Object,_mapper);

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
            var mockNodeService = new Mock<IBchNodeService>();

            //todo setup
            var blockProvider = new BchBlockProviderService(mockNodeService.Object, _logger.Object, _mapper);

            // act
            Func<Task> f = async () => { await blockProvider.GetBlockAsync(blockNr.ToString()); };

            // assert
            await f.Should().ThrowAsync<DllNotFoundException>();
        }

        public BchBlockResponse GenerateBlockResponse(int nr)
        {
            var block = new BchBlockResponse()
            {
                Hash = nr.ToString(),
                Tx = new List<BchBlockTxResponse>()
            };
            return block;
        }


        /*
         *         [Fact]
        public async Task GetBestBlock_ConnectionCanBeMade_SuccesReturnHash()
        {
            // arrange
            var service = new BtcCoreService(_mockHttpClientFactory.Object, _mockLogger.Object);

            // act
            Func<Task> f = async () => { await service.GetBestBlockHashAsync(); };


            // assert
            await f.Should().ThrowAsync<NullReferenceException>();
            _mockLogger.Verify(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Connection to Btc Core could not be made", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<NullReferenceException>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once
            );
        }
         */

    }
}
