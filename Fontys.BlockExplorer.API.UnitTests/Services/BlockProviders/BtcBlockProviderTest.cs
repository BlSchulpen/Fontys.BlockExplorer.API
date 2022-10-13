using AutoMapper;
using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.NodeDataManager.AutomapProfiles;
using Moq;
using System.Threading.Tasks;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Xunit;
using Fontys.BlockExplorer.API.UnitTests.Factories.ResponseFactories;

namespace Fontys.BlockExplorer.API.UnitTests.Services.BlockProviders
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

        [Fact]
        public async Task GetBlock_BlockExists_ReturnBlock()
        {
            //arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<BtcProfile>());
            var mapper = new Mapper(config);

            const int blockNr = 0;
            var mockNodeService = GetMockNodeService(blockNr);

            var service = new BtcBlockProviderService(mockNodeService.Object, mapper);

            //act
            var result = await service.GetBlockAsync(blockNr.ToString());

            //assert
            result.Hash.Should().Be(blockNr.ToString());
        }


        //Todo determine why exception instead of returning null 
        private Mock<IBtcNodeService> GetMockNodeService(int blockNr)
        {
            const int nrInputs = 3;
            const int nrOutputs = 3;
            const int nrTransactions = 1; //TODO Find a way to give unique mock responses for different parameters
            var btcBlockResponse = _blockFactory.BlockResponse(blockNr, nrTransactions);
            var mockNodeService = new Mock<IBtcNodeService>();
            mockNodeService.Setup(nodeService => nodeService.GetBlockFromHashAsync(blockNr.ToString())).ReturnsAsync(btcBlockResponse);
            for (var i = 0; i < nrTransactions; i++)
            {
                var transactionResponse = _transactionFactory.BtcTransactionResponse(nrInputs, nrOutputs, i);
                mockNodeService.Setup(nodeService => nodeService.GetRawTransactionAsync(i.ToString())).ReturnsAsync(transactionResponse);
            }
            return mockNodeService;
        }
    }
}