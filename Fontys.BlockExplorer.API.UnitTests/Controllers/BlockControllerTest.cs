using AutoMapper;
using Fontys.BlockExplorer.API.Controllers;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Application.Services.BlockService;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Controllers
{
    public class BlockControllerTest
    {
        [Fact]
        public async Task Get_Block_Call_Service()
        {
            // arrange
            var hash = "Test";
            var mockService = new Mock<IBlockService>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            var controller = new BlockController(mockService.Object, mapper);

            // act
            await controller.GetBlockAsync(hash);

            // assert
            mockService.Verify(f => f.GetBlockAsync(It.Is<string>(x => x == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
