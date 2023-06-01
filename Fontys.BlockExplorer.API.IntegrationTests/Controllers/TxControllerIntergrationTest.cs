using AutoMapper;
using FluentAssertions;
using Fontys.BlockExplorer.API.Controllers;
using Fontys.BlockExplorer.API.Profiles;
using Fontys.BlockExplorer.Application.Services.TxService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Fontys.BlockExplorer.API.IntegrationTests.Controllers
{
    public class TxControllerIntergrationTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;
        private readonly Mock<ILogger<ExplorerTransactionService>> _logger;

        public TxControllerIntergrationTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _logger = new Mock<ILogger<ExplorerTransactionService>>();
        }

        [Fact]
        public void Get_Transaction_Existing()
        {
            // arrange
            var nrTransactions = 1;
            var hash = "0";
            var controller = SetupController(nrTransactions);

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
            var hash = "0";
            var controller = SetupController(nrTransactions);

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
                var transaction = new Transaction() { Hash = i.ToString(), Inputs = new List<TxInput>(), Outputs = new List<TxOutput>() };
                newTransactions.Add(transaction);
            }
            return newTransactions;
        }

        private TransactionController SetupController(int nrTransactions)
        {
            var transactions = MockTransactions(nrTransactions);
            _dbContextMock.Setup(x => x.Transactions).ReturnsDbSet(transactions);
            var service = new ExplorerTransactionService(_dbContextMock.Object, _logger.Object);
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ExplorerProfile>());
            var mapper = new Mapper(config);
            var controller = new TransactionController(service, mapper);
            return controller;
        }
    }
}
