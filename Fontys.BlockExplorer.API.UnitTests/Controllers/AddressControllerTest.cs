namespace Fontys.BlockExplorer.API.UnitTests.Controllers
{
    using Fontys.BlockExplorer.API.Controllers;
    using Fontys.BlockExplorer.Application.Services.AddressService;
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
            mockService.Verify(x => x.GetAddressAsync(It.Is<string>(x => x == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
