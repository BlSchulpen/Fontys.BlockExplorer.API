using AutoMapper;
using FluentAssertions;
using Fontys.BlockExplorer.API.Controllers;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.BlockService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

namespace Fontys.BlockExplorer.API.IntegrationTests.Controllers
{
    public class BlockControllerIntergrationTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;
        private readonly Mock<IAddressRestoreService> _mockAddressRestoreService;
        private readonly Mock<ILogger<ExplorerBlockService>> _mockBlockService;

        public BlockControllerIntergrationTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _mockAddressRestoreService = new Mock<IAddressRestoreService>();
            _mockBlockService = new Mock<ILogger<ExplorerBlockService>>();

        }

        [Fact]
        public void Get_Block_Not_Found()
        {
            // arrange
            var nrBlocks = 0;
            var hash = "0";
            var controller = SetupController(nrBlocks);

            // act
            var response = controller.GetBlockAsync(hash);

            // assert
            var result = response.Result as NotFoundResult;
            result.Should().NotBeNull();
        }

        private BlockController SetupController(int nrBlocks)
        {
            var blocks = MockBlocks(nrBlocks);
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(blocks);
            var service = new ExplorerBlockService(_dbContextMock.Object, _mockAddressRestoreService.Object, _mockBlockService.Object);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            var controller = new BlockController(service, mapper);
            return controller;
        }

        private List<Block> MockBlocks(int nrTransactions)
        {
            var newBlocks = new List<Block>();
            for (var i = 0; i < nrTransactions; i++)
            {
                var transaction = new Transaction() { Hash = "0", Inputs = new List<TxInput>(), Outputs = new List<TxOutput>() };
                var transactions = new List<Transaction>() { transaction };
                newBlocks.Add(new Block() { Height = i, Hash = i.ToString(), Transactions = transactions });
            }
            return newBlocks;
        }
    }
}
