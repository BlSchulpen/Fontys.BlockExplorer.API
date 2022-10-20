using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Eth;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fontys.BlockExplorer.NodeWarehouse.UnitTests
{
    public class EthGethServiceTest
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<EthGethService>> _mockLogger;

        public EthGethServiceTest()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockClient = new Mock<HttpClient>();
            _mockHttpClientFactory.Setup(x => x.CreateClient("ethGeth")).Returns(mockClient.Object);
            _mockLogger = new Mock<ILogger<EthGethService>>();
        }

        /*
        [Fact]
        public async Task GetLatestNr_ConnectionNotEstablished_LogConnectionError()
        {
            // arrange
            var service = new EthGethService(_mockHttpClientFactory.Object, _mockLogger.Object);

            // act
            Func<Task> f = async () => { await service.GetLatestNumber(); };


            // assert
            await f.Should().ThrowAsync<NullReferenceException>();
            _mockLogger.Verify(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Connection to Eth Geth could not be made", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<NullReferenceException>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once
            );
        }
        */
    }
}
