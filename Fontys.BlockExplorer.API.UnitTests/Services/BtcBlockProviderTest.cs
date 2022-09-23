using BenchmarkDotNet.Configs;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Xunit;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class BtcBlockProviderTest
    {
        [Fact]
        public async Task GetBlock_BlockExists_ReturnBlock()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Block, BlockResponse>());
            var mapper = new Mapper(config);
            const string hash = "0000";
            var blockInChain = new Block() { Hash = hash };

            mapper.Should().NotBeNull();
        }
    }
}
