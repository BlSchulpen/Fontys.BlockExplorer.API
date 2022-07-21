namespace Fontys.BlockExplorer.API.Test.Controllers
{
    using Xunit;
    using Fontys.BlockExplorer.Application.Services.BlockService;
    using Moq;
    using Fontys.BlockExplorer.API.Controllers;
    using System.Threading.Tasks;
    using Fontys.BlockExplorer.Domain.CQS;

    public class BlockControllerTest
    {
        [Fact]
        public async Task Get_Block_Call_Service()
        {
            // arrange
            var hash = "Test";
            var mockService = new Mock<IBlockService>();
            var controller = new BlockController(mockService.Object);

            // act
            await controller.GetBlock(hash);

            // assert
            mockService.Verify(f => f.GetBlockAsync(It.Is<GetBlockCommand>(x => x.Hash == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
