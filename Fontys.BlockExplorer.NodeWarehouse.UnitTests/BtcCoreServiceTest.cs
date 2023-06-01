using FluentAssertions;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace Fontys.BlockExplorer.NodeWarehouse.UnitTests
{
    public class BtcCoreServiceTest
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<BtcCoreService>> _mockLogger;
        private readonly Mock<HttpClient> _mockClient;

        public BtcCoreServiceTest()
        {
            _mockClient = new Mock<HttpClient>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClientFactory.Setup(x => x.CreateClient("BtcCore")).Returns(_mockClient.Object);
            _mockLogger = new Mock<ILogger<BtcCoreService>>();
        }

        [Fact]
        public async Task GetBestBlock_NoConnectionMade_LogNullException()
        {
            // arrange
            var service = new BtcCoreService(_mockHttpClientFactory.Object, _mockLogger.Object);

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

        /*
        [Fact]
        public async Task GetBestBlock_ConnectionCanBeMade_SuccesReturnHash()
        {
            // arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var mockProtected = mockMessageHandler.Protected();
            var setupApiRequest = mockProtected.Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );
            var apiMockedResponse =
                setupApiRequest.ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("mocked API response")
                });

           // var service = new BtcCoreService(mockProtected., _mockLogger.Object);
            //            nodeService.Setup(x => x.GetBlockFromHashAsync(genesisBlock.Hash)).ReturnsAsync(genesisBlock);

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