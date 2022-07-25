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

    public class BlockControllerIntergrationTest
    {
        private readonly MockDbFactory _dbFactory;
        private readonly Mock<BlockExplorerContext> _dbContextMock;

        public BlockControllerIntergrationTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _dbFactory = new MockDbFactory();
        }

        //todo maybe add new intergration test with database connectivity --> API request        
        [Fact]
        public void Get_Block_Existing()
        {
            // arrange
            var nrBlocks = 1;
            var blocks = _dbFactory.MockBlocks(nrBlocks);
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(blocks);
            var service = new ExplorerBlockService(_dbContextMock.Object);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
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
    }
}
