using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Fontys.BlockExplorer.Application.Services.AddressRestoreService;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace Fontys.BlockExplorer.API.UnitTests.Services
{
    public class NodeMonitoringServiceTest
    {
        private readonly Mock<BlockExplorerContext> _dbContextMock;
        private readonly Mock<Func<CoinType, IBlockDataProviderService>> _blockDataProviderResolverMock;
        private readonly Mock<IBlockDataProviderService> _blockDataProviderServiceMock;
        public NodeMonitoringServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _blockDataProviderServiceMock = new Mock<IBlockDataProviderService>();
            _blockDataProviderResolverMock = new Mock<Func<CoinType, IBlockDataProviderService>>();
            _blockDataProviderResolverMock.Setup(x => x(CoinType.BTC)).Returns(_blockDataProviderServiceMock.Object);
        }

        [Fact]
        public async Task RemoveBadBlock_NonStored_ReturnEmptyList()
        {
            //arrange
            _dbContextMock.Setup(context => context.Blocks).ReturnsDbSet(new List<Block>());
            var mockAddressRestoreService = new Mock<IAddressRestoreService>();
            var service = new NodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, mockAddressRestoreService.Object);
            const CoinType coinType = CoinType.BTC;

            //act
            var removedBlocks = await service.RemoveBadBlocksAsync(coinType);

            //assert
            removedBlocks.Should().BeEmpty();
        }

        [Fact]
        public async Task RemoveBadBlock_NonBadStored_ReturnEmptyList()
        {
            //arrange
            const int nrStored = 3;
            const int latestBlock = nrStored - 1;
            var stored = GetStoredBlocks(nrStored);
            _blockDataProviderServiceMock.Setup(b => b.GetHashFromHeightAsync(latestBlock)).ReturnsAsync(latestBlock.ToString());
            _blockDataProviderServiceMock.Setup(b => b.GetBlockAsync(latestBlock.ToString())).ReturnsAsync(stored.FirstOrDefault(b => b.Hash == latestBlock.ToString()));
            _dbContextMock.Setup(context => context.Blocks).ReturnsDbSet(stored);
            var mockAddressRestoreService = new Mock<IAddressRestoreService>();
            var service = new NodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, mockAddressRestoreService.Object);
            const CoinType coinType = CoinType.BTC;

            //act
            var removedBlocks = await service.RemoveBadBlocksAsync(coinType);

            //assert
            removedBlocks.Should().BeEmpty();
        }



        private List<Block> GetStoredBlocks(int nrBlocks)
        {
            var storedBlocks = new List<Block>();
            for (var i = 0; i < nrBlocks; i++)
            {
                var block = new Block() { Hash = i.ToString(), CoinType = CoinType.BTC, Height = i, NetworkType = NetworkType.BtcMainet};
                storedBlocks.Add(block);
            }

            var seqBlocks = storedBlocks.Where(b => b.Height != 0); //think of a better name
            foreach (var block in seqBlocks)
            {
                block.PreviousBlockHash = (block.Height - 1).ToString();
            }
            return storedBlocks;
        }
    }
}
