namespace Fontys.BlockExplorer.API.IntegrationTests.Controllers
{
    using AutoMapper;
    using Fontys.BlockExplorer.API.Controllers;
    using Fontys.BlockExplorer.Application.Services.TxService;
    using Fontys.BlockExplorer.Data;
    using Fontys.BlockExplorer.Domain.Models;
    using Moq;
    using Moq.EntityFrameworkCore;
    using System.Collections.Generic;
    using Xunit;
    using Microsoft.AspNetCore.Mvc;
    using FluentAssertions;
    using Fontys.BlockExplorer.API.Profiles;

    public class TxControllerIntergrationTest
    {
        private readonly MockDbFactory _dbFactory;
        private readonly Mock<BlockExplorerContext> _dbContextMock;

        public TxControllerIntergrationTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _dbFactory = new MockDbFactory();
        }

        //todo maybe add new intergration test with database connectivity --> API request        
        [Fact]
        public void Get_Transaction_Existing()
        {
            // arrange
            var nrTransactions = 1;
            var transactions = MockTransactions(nrTransactions);
            _dbContextMock.Setup(x => x.Transactions).ReturnsDbSet(transactions);
            var service = new ExplorerTxService(_dbContextMock.Object);
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ExplorerProfile>());
            var mapper = new Mapper(config);
            var controller = new TxController(service, mapper);
            string hash = "0";

            // act
           var response = controller.GetTransaction(hash);

            // assert
            var result = response.Result as OkObjectResult;
            result.Should().NotBeNull();
        }

        [Fact]
        public void Get_Transaction_NotFound()
        {
            // arrange
            var nrTransactions = 0;
            var transactions = MockTransactions(nrTransactions);
            _dbContextMock.Setup(x => x.Transactions).ReturnsDbSet(transactions);
            var service = new ExplorerTxService(_dbContextMock.Object);
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ExplorerProfile>());
            var mapper = new Mapper(config);
            var controller = new TxController(service, mapper);
            string hash = "0";

            // act
            var response = controller.GetTransaction(hash);

            // assert
            var result = response.Result as NotFoundResult;
            result.Should().NotBeNull();
        }

        private List<Transaction> MockTransactions(int nrTransactions)
        {
            var newTransactions = new List<Transaction>();
            for (int i = 0; i < nrTransactions; i++)
            {
                var transaction = new Transaction() { Hash = i.ToString(), Transfers = new List<Transfer>() };
                newTransactions.Add(transaction);
            }
            return newTransactions;
        }    
    }
}
