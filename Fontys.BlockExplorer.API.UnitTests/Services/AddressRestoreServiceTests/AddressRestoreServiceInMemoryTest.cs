using System.Threading.Tasks;
using FluentAssertions;
using Fontys.BlockExplorer.API.UnitTests.Factories;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Data.InMemory;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.AddressRestoreServiceTests
{
    public class AddressRestoreServiceInMemoryTest
    {
        private const int NrStored = 100;
        private InMemoryDatabaseContext _inMemoryDatabaseContext;

        [Fact]
        public async Task TestRestoreAddresses_SomeAlreadyInDB_ReturnNonStoredAddresses()
        {
            // arrange
            SetUpDb();
            AddAddressesInDb();
            const int nrNewAddresses = 10;
            var newAddresses = BlockFactory.NewAddresses(NrStored, nrNewAddresses);
            var service = new ExplorerAddressRestoreService(_inMemoryDatabaseContext);
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
            const int nrNewAddresses = 10;
            var newAddresses = BlockFactory.NewAddresses(NrStored, nrNewAddresses);
            var service = new ExplorerAddressRestoreService(_inMemoryDatabaseContext);
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
