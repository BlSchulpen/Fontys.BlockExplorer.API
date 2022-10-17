﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Fontys.BlockExplorer.API.UnitTests.Factories;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.AddressService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Data.InMemory;
using Fontys.BlockExplorer.Data.PostgresDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services.AddressRestoreServiceTests
{
    // Address restore service is also tested with an in memory DB
    // It is best practice not to use In memory DB for most unit tests but I needed to test if dupplicates could be added to the DB 
    public class AddressRestoreServiceInMemoryTest
    {
        private readonly BlockExplorerContext _inMemoryContext;
        private const int NrStored = 100;
        private InMemoryDatabaseContext _inMemoryDatabaseContext;
    

        [Fact]
        public async Task TestRestoreAddresses_SomeAlreadyInDB_ReturnNonStoredAddresses()
        {
            // arrange
            SetUpDb(); //TODO no Setup and TearDown function in NUnit consider creating a disposable base class
            const int nrNewAddresses = 10;
            var newAddresses = BlockFactory.NewAddresses(NrStored, nrNewAddresses);
            var service = new ExplorerAddressRestoreService(_inMemoryContext);
            var newBlock = BlockFactory.NewBlock(newAddresses);

            // act
            var newlyStored = await service.RestoreAddressesAsync(newBlock);

            // assert
            newlyStored.Should().BeEquivalentTo(newAddresses);
            TearDownDb();
        }

        [Fact]
        public async Task TestRestoreAddresses_NoAddressesInDb_ReturnNonStoredAddresses()
        {
            // arrange
            SetUpDb();
            const int nrNewAddresses = 10;
            var newAddresses = BlockFactory.NewAddresses(NrStored, nrNewAddresses);
            var service = new ExplorerAddressRestoreService(_inMemoryContext);
            var newBlock = BlockFactory.NewBlock(newAddresses);
            
            // act


            // assert
            TearDownDb();

        }

        private void SetUpDb()
        {
            var options = new DbContextOptionsBuilder<InMemoryDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "BlockDatabase")
                .Options;
            _inMemoryDatabaseContext = new InMemoryDatabaseContext(options);
            var storedAddresses = BlockFactory.StoredAddresses(NrStored);
            _inMemoryContext.Addresses.AddRange(storedAddresses);
            _inMemoryContext.SaveChanges();
        }

        private void TearDownDb()
        {
            _inMemoryDatabaseContext.Database.EnsureDeleted();
        }
    }
}
