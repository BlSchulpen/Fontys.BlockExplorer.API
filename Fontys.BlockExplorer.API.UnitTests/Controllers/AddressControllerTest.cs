namespace Fontys.BlockExplorer.API.UnitTests.Controllers
{
    using Fontys.BlockExplorer.API.Controllers;
    using Fontys.BlockExplorer.Application.Services.AddressService;
    using Fontys.BlockExplorer.Domain.CQS;
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
            var controller = new AddressController(mockService.Object);

            // act
            await controller.GetAddress(hash);

            // assert
            mockService.Verify(x => x.GetAddressAsync(It.Is<GetAddressCommand>(x => x.Hash == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
