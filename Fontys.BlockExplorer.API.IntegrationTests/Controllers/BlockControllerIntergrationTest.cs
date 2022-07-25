namespace Fontys.BlockExplorer.API.IntegrationTests.Controllers
{
    using FluentAssertions;
    using Fontys.BlockExplorer.API.Controllers;
    using Fontys.BlockExplorer.Data;
    using Moq;
    using Moq.EntityFrameworkCore;
    using Xunit;
    using Fontys.BlockExplorer.Application.Services.BlockService;
    using AutoMapper;
    using Fontys.BlockExplorer.Domain.Models;
    using Fontys.BlockExplorer.API.Dto.Response;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Fontys.BlockExplorer.API.Profiles;

    public class BlockControllerIntergrationTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;

        public BlockControllerIntergrationTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
        }

        //todo maybe add new intergration test with database connectivity --> API request        
        [Fact]
        public void Get_Block_Existing()
        {
            // arrange
            var nrBlocks = 1;
            var blocks = MockBlocks(nrBlocks);
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(blocks);
            var service = new ExplorerBlockService(_dbContextMock.Object);
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ExplorerProfile>());
            var mapper = new Mapper(config);
            var controller = new BlockController(service, mapper);
            string hash = "0";

            // act
            var response = controller.GetBlockAsync(hash);

            // assert
            var result = response.Result as OkObjectResult;
            result.Should().NotBeNull();
        }


        [Fact]
        public void Get_Block_Not_Found()
        {
            // arrange
            var blocks = new List<Block>();
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(blocks);
            var service = new ExplorerBlockService(_dbContextMock.Object);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            var controller = new BlockController(service, mapper);
            string hash = "0";

            // act
            var response = controller.GetBlockAsync(hash);

            // assert
            var result = response.Result as NotFoundResult;
            result.Should().NotBeNull();
        }

        private List<Block> MockBlocks(int nrTransactions)
        {
            var newBlocks = new List<Block>();
            for (int i = 0; i < nrTransactions; i++)
            {
                var transaction = new Transaction() { Hash = "0", Transfers = new List<Transfer>() };
                var transactions = new List<Transaction>() { transaction };
                newBlocks.Add(new Block() { Height = i, Hash = i.ToString(), Transactions = transactions });
            }
            return newBlocks;
        }
    }
}
