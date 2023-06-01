using FluentAssertions;
using Fontys.BlockExplorer.API.UnitTests.Factories;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.AddressService;
using Fontys.BlockExplorer.Data.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.AddressRestoreServiceTests
{
    public class AddressRestoreServiceInMemoryTest
    {
        private const int NrStored = 100;
        private InMemoryDatabaseContext _inMemoryDatabaseContext;
        private readonly Mock<IAddressService> _mockExplorerAddressService;
        private readonly Mock<ILogger<ExplorerAddressRestoreService>> _logger;

        public AddressRestoreServiceInMemoryTest()
        {
            _mockExplorerAddressService = new Mock<IAddressService>();
            _logger = new Mock<ILogger<ExplorerAddressRestoreService>>();
        }

        [Fact]
        public async Task TestRestoreAddresses_SomeAlreadyInDB_ReturnNonStoredAddresses()
        {
            // arrange
            SetUpDb();
            AddAddressesInDb();
            const int nrNewAddresses = 10;
            var newAddresses = BlockFactory.NewAddresses(NrStored, nrNewAddresses);
            var service = new ExplorerAddressRestoreService(_inMemoryDatabaseContext, _mockExplorerAddressService.Object, _logger.Object);
            var newBlock = BlockFactory.NewBlock(newAddresses);

            // act
            var newlyStored = await service.RestoreAddressesAsync(newBlock);

            // assert
            newlyStored.Should().BeEquivalentTo(newAddresses);
            TearDownDb();
        }

        [Fact]
        public async Task TestRestoreAddresses_NoAddressesInDb_ReturnNewStoredAddresses()
        {
            // arrange
            SetUpDb();
            AddAddressesInDb();
            const int nrNewAddresses = 10;
            var newAddresses = BlockFactory.NewAddresses(NrStored, nrNewAddresses);
            var service = new ExplorerAddressRestoreService(_inMemoryDatabaseContext, _mockExplorerAddressService.Object, _logger.Object);
            var newBlock = BlockFactory.NewBlock(newAddresses);

            // act
            var newlyStored = await service.RestoreAddressesAsync(newBlock);

            // assert
            newlyStored.Should().BeEquivalentTo(newAddresses);
            TearDownDb();
        }


        private void SetUpDb()
        {
            var options = new DbContextOptionsBuilder<InMemoryDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "BlockDatabase")
                .Options;
            _inMemoryDatabaseContext = new InMemoryDatabaseContext(options);
        }

        private void AddAddressesInDb()
        {
            var storedAddresses = BlockFactory.StoredAddresses(NrStored);
            _inMemoryDatabaseContext.Addresses.AddRange(storedAddresses);
            _inMemoryDatabaseContext.SaveChanges();
        }

        private void TearDownDb()
        {
            _inMemoryDatabaseContext.Database.EnsureDeleted();
        }
    }
}
