using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.AddressService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class AddressServiceTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;
        private readonly Mock<ILogger<ExplorerAddressService>> _mockLogger;

        public AddressServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _mockLogger = new Mock<ILogger<ExplorerAddressService>>();
        }

        [Fact]
        public async Task GetAddress_AddressExists_ReturnStored()
        {
            // arrange
            const string storedAddressHash = "0000";
            var storedAddress = new Address() { Hash = storedAddressHash };
            var storedAddresses = new List<Address>() { storedAddress };
            var mockLogger = new Mock<ILogger<ExplorerAddressService>>();
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(storedAddresses);
            var service = new ExplorerAddressService(_dbContextMock.Object, mockLogger.Object);

            // act
            var returnedAddress = await service.GetAddressAsync(storedAddressHash);

            // assert
            returnedAddress.Should().BeEquivalentTo(storedAddress);
        }

        [Fact]
        public async Task GetAddress_AddressNotExists_Test()
        {
            // arrange
            const string addressHash = "0000";
            _dbContextMock.Setup(x => x.Addresses).ReturnsDbSet(new List<Address>());
            var mockLogger = new Mock<ILogger<ExplorerAddressService>>();
            var service = new ExplorerAddressService(_dbContextMock.Object, mockLogger.Object);

            // act
            var returnedAddress = await service.GetAddressAsync(addressHash);

            // assert
            returnedAddress.Should().BeNull();
        }
    }
}
