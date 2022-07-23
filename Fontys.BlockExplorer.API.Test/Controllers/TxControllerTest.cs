namespace Fontys.BlockExplorer.API.UnitTest.Controllers
{
    using AutoMapper;
    using Fontys.BlockExplorer.API.Controllers;
    using Fontys.BlockExplorer.API.Dto.Response;
    using Fontys.BlockExplorer.Application.Services.TxService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            var controller = new TxController(mockService.Object,mapper);

            // act
            await controller.GetTransaction(hash);

            // assert
            mockService.Verify(x => x.GetTransactionAsync(It.Is<GetTxCommand>(x => x.Hash == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
