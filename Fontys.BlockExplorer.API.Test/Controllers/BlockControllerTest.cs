﻿namespace Fontys.BlockExplorer.API.UnitTest.Controllers
{
    using Xunit;
    using Fontys.BlockExplorer.Application.Services.BlockService;
    using Moq;
    using Fontys.BlockExplorer.API.Controllers;
    using System.Threading.Tasks;
    using Fontys.BlockExplorer.Domain.CQS;
    using Fontys.BlockExplorer.Domain.Models;
    using Fontys.BlockExplorer.API.Dto.Response;
    using AutoMapper;

    public class BlockControllerTest
    {
        [Fact]
        public async Task Get_Block_Call_Service()
        {
            // arrange
            var hash = "Test";
            var mockService = new Mock<IBlockService>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            var controller = new BlockController(mockService.Object, mapper);

            // act
            await controller.GetBlock(hash);

            // assert
            mockService.Verify(f => f.GetBlockAsync(It.Is<GetBlockCommand>(x => x.Hash == hash)));
            mockService.VerifyNoOtherCalls();
        }
    }
}
