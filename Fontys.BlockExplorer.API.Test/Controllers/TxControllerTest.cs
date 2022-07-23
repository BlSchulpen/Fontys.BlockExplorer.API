namespace Fontys.BlockExplorer.API.UnitTest.Controllers
{
    using Fontys.BlockExplorer.API.Controllers;
    using Fontys.BlockExplorer.Application.Services.TxService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Moq;
    using System.Threading.Tasks;
    using Xunit;

    public class TxControllerTest
    {
        [Fact]
        public async Task Get_Tx_Call_Service()
        {
            // arrange
            var hash = "Test";
            var mockService = new Mock<ITxService>();
            var controller = new TxController(mockService.Object);

            // act
            await controller.GetTransaction(hash);

            // assert
            mockService.Verify(x => x.GeTransactionAsync(It.Is<GetTxCommand>(x => x.Hash == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
