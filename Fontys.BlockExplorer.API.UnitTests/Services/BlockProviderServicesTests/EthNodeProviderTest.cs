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
            var blockProvider = new EthBlockProviderService(mockNodeService.Object, _mapper, _logger.Object);

            // act
            var responseBlock = await blockProvider.GetBlockAsync(blockNr.ToString());

            // assert
            responseBlock.Should().BeEquivalentTo(responseBlock);
        }

        [Fact]
        public async Task RequestGenesisBlock_BlockNotExists_LogsError()
        {
            // arrange
            const int blockNr = 0;
            var mockNodeService = new Mock<IEthNodeService>();
            var blockProvider = new EthBlockProviderService(mockNodeService.Object, _mapper, _logger.Object);

            // act
            Func<Task> f = async () => { await blockProvider.GetBlockAsync(blockNr.ToString()); };

            // assert
            _logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => string.Equals("Could not retrieve ETH block {Nr}", blockNr.ToString(), StringComparison.InvariantCultureIgnoreCase)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),Times.Once);
             await f.Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task RequestBestBlock_OneExists_ReturnBlockSuccessfully()
        {
            // arrange
            const int blockNr = 0;
            var expectedResponse = GenerateBlockResponse(blockNr);
            var mockNodeService = new Mock<IEthNodeService>();         
            mockNodeService.Setup(x => x.GetLatestNumber()).ReturnsAsync(blockNr);
            var responseBlock = GenerateBlockResponse(blockNr);
            mockNodeService.Setup(x => x.GetBlockByNumberAsync(blockNr)).ReturnsAsync(responseBlock);

            //todo setup
            var blockProvider = new EthBlockProviderService(mockNodeService.Object, _mapper, _logger.Object);

            // act
            var responseHash = await blockProvider.GetBestBlockHashAsync();

            // assert
            responseHash.Should().Be(expectedResponse.Hash);
        }

        [Fact]
        public async Task RequestBestBlock_MultipleExists_ReturnBlockSuccessfully()
        {
            // arrange
            const int blockNr = 5;
            var expectedResponse = GenerateBlockResponse(blockNr);
            var mockNodeService = new Mock<IEthNodeService>();
            mockNodeService.Setup(x => x.GetLatestNumber()).ReturnsAsync(blockNr);

            //todo setup
            var blockProvider = new EthBlockProviderService(mockNodeService.Object, _mapper, _logger.Object);

            // act
            var responseHash = await blockProvider.GetBestBlockHashAsync();

            // assert
            responseHash.Should().Be(expectedResponse.Hash);
        }


        //TODO is this edge case an issue --> will now return old block but new blocks are gathered ~1 minute after
        [Fact]
        public async Task RequestBestBlock_BlockAddedAfterRequestingLatestNumber_ReturnOldBlock()
        {
            // arrange
            const int oldLatest = 5;
            const int newLatest = oldLatest+1;

            var expectedResponse = GenerateBlockResponse(oldLatest);
            var newLatestResponse = GenerateBlockResponse(newLatest);

            var mockNodeService = new Mock<IEthNodeService>();
            mockNodeService.Setup(x => x.GetLatestNumber()).ReturnsAsync(oldLatest);
            mockNodeService.Setup(x => x.GetBlockByNumberAsync(newLatest)).ReturnsAsync(newLatestResponse);
            mockNodeService.Setup(x => x.GetBlockByNumberAsync(oldLatest)).ReturnsAsync(expectedResponse);
            var blockProvider = new EthBlockProviderService(mockNodeService.Object, _mapper, _logger.Object);

            // act
            var responseHash = await blockProvider.GetBestBlockHashAsync();

            // assert
            responseHash.Should().Be(expectedResponse.Hash);
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
