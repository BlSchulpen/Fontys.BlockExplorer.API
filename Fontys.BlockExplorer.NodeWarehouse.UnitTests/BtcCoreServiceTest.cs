using FluentAssertions;
using Fontys.BlockExplorer.NodeWarehouse.Configurations;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xunit;
using Moq.Protected;

namespace Fontys.BlockExplorer.NodeWarehouse.UnitTests
{
    public class BtcCoreServiceTest
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<BtcCoreService>> _mockLogger;
        private readonly Mock<HttpClient> _mockClient;

        public BtcCoreServiceTest()
        {
         //   _mockClient = new Mock<HttpClient>();
            //TODO

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("test", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });


            var client = new HttpClient(mockHttpMessageHandler.Object);

           // _mockClient.Setup(x => x.PostAsync(It.IsAny<string>(),It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.EarlyHints });
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClientFactory.Setup(x => x.CreateClient("BtcCore")).Returns(client);
            _mockLogger = new Mock<ILogger<BtcCoreService>>();
        }

        /*
        [Fact]
        public async Task GetBestBlock_NoConnectionMade_LogNullException()
        {
            // arrange
            var bitCoinRequestConfiguration = new BitcoinRequestConfiguration();

            var service = new BtcCoreService(_mockHttpClientFactory.Object, _mockLogger.Object, bitCoinRequestConfiguration);

            // act
            Func<Task> f = async () => { await service.GetBestBlockHashAsync(); };


            // assert
            await f.Should().ThrowAsync<Exception>();
            _mockLogger.Verify(x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((o, t) => string.Equals("Message could not be send the following error was thrown ", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                    Times.Once
            );
        }
        */

        [Fact]
        public async Task GetBestBlock_GetBlock_Success_ReturnBlock() 
        {
            // arrange
            var bitCoinRequestConfiguration = new BitcoinRequestConfiguration();
            var service = new BtcCoreService(_mockHttpClientFactory.Object, _mockLogger.Object, bitCoinRequestConfiguration);
            var bestBlockHash = "000000000000000000051ab8e0f19a44add8e4a831d55565f6a6e0e82fda4a62";

            // act 
            var result = await service.GetBestBlockHashAsync();


            // assert
            result.Should().BeEquivalentTo(bestBlockHash); 
        }
    }
}