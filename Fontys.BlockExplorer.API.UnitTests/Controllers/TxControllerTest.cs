using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Controllers
{
    public class TxControllerTest
    {
        [Fact]
        public async Task Get_Tx_Call_Service()
        {
            /*
            // arrange
            var hash = "Test";
            var mockService = new Mock<ITransactionService>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            var controller = new TransactionController(mockService.Object, mapper);

            // act
            await controller.GetTransaction(hash);

            // assert
            mockService.Verify(x => x.GetTransactionAsync(It.Is<string>(x => x == hash)));
            mockService.VerifyNoOtherCalls();
            */
        }
    }
}
