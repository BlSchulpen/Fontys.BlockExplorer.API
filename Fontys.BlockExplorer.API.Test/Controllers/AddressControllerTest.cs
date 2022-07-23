namespace Fontys.BlockExplorer.API.UnitTest.Controllers
{
    using AutoMapper;
    using Fontys.BlockExplorer.API.Controllers;
    using Fontys.BlockExplorer.API.Dto.Response;
    using Fontys.BlockExplorer.Application.Services.AddressService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;
    using Moq;
    using System.Threading.Tasks;
    using Xunit;

    public class AddressControllerTest
    {
        [Fact]
        public async Task Get_Address_Call_Service()
        {
            // arrange
            var hash = "Test";
            var mockService = new Mock<IAddressService>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            var controller = new AddressController(mockService.Object, mapper);

            // act
            await controller.GetAddress(hash);

            // assert
            mockService.Verify(x => x.GetAddressAsync(It.Is<GetAddressCommand>(x => x.Hash == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
