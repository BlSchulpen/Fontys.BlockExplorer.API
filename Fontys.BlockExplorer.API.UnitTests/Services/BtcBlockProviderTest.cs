using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Xunit;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.API.UnitTests.Factories;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices;
using Moq;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class BtcBlockProviderTest
    {
        private readonly BtcCoreBlockResponseFactory _blockFactory;
        private readonly BtcCoreTransactionResponseFactory _transactionFactory;

        public BtcBlockProviderTest()
        {
            _blockFactory = new BtcCoreBlockResponseFactory();
            _transactionFactory = new BtcCoreTransactionResponseFactory();
        }
    
        /*
        [SetUp]
        public void Init()
        {
            factory = new BtcCoreBlockResponseFactory();
        }
        */

        [Fact]
        public async Task GetBlock_BlockExists_ReturnBlock()
        {
            //arrange
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            const int blockNr = 0;
            var mockNodeService = GetMockNodeService(blockNr);

            var service = new BtcBlockProviderService(mockNodeService.Object,mapper);

            //act
            var result = service.GetBlockAsync(blockNr.ToString());

            //assert
            mapper.Should().NotBeNull();
        }

        private Mock<IBtcNodeService> GetMockNodeService(int blockNr)
        {
            const int nrInputs = 3;
            const int nrOutputs = 3;
            const int nrTransactions = 5;
            var btcBlockResponse = _blockFactory.BlockResponse(blockNr, nrTransactions);
            var mockNodeService = new Mock<IBtcNodeService>();
            mockNodeService.Setup(nodeService => nodeService.GetBlockFromHashAsync(blockNr.ToString())).ReturnsAsync(btcBlockResponse);
            for (var i = 0; i < nrTransactions; i++)
            {
                var transactionResponse = _transactionFactory.BtcTransactionResponse(nrInputs,nrOutputs,i);
                mockNodeService.Setup(nodeService => nodeService.GetRawTransactionAsync(i.ToString())).ReturnsAsync(transactionResponse);
            }
            return mockNodeService;
        }
    }
}
