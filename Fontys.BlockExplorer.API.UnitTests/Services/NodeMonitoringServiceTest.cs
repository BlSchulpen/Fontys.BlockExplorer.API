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
        private readonly Mock<IAddressRestoreService> _mockAddressRestoreService;

        public NodeMonitoringServiceTest()
        {
            _dbContextMock = new Mock<BlockExplorerContext>();
            _blockDataProviderServiceMock = new Mock<IBlockDataProviderService>();
            _blockDataProviderResolverMock = new Mock<Func<CoinType, IBlockDataProviderService>>();
            _blockDataProviderResolverMock.Setup(x => x(CoinType.BTC)).Returns(_blockDataProviderServiceMock.Object);
            _mockAddressRestoreService = new Mock<IAddressRestoreService>();
        }

        [Fact]
        public async Task RemoveBadBlock_NonStored_ReturnEmptyList()
        {
            //arrange
            _dbContextMock.Setup(context => context.Blocks).ReturnsDbSet(new List<Block>());
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);
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
            var stored = GetChainBlocks(nrStored);
            _blockDataProviderServiceMock.Setup(b => b.GetHashFromHeightAsync(latestBlock)).ReturnsAsync(latestBlock.ToString());
            _blockDataProviderServiceMock.Setup(b => b.GetBlockAsync(latestBlock.ToString()))!.ReturnsAsync(stored.FirstOrDefault(b => b.Hash == latestBlock.ToString()));
            _dbContextMock.Setup(context => context.Blocks).ReturnsDbSet(stored);
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);
            const CoinType coinType = CoinType.BTC;

            //act
            var removedBlocks = await service.RemoveBadBlocksAsync(coinType);

            //assert
            removedBlocks.Should().BeEmpty();
        }

        [Fact]
        public async Task RemoveBadBlock_BadStored_ReturnsBadBlocks()
        {
            //arrange
            const CoinType coinType = CoinType.BTC;
            const int nrChainBlocks = 3;
            const int nrBadStored = 2;
            var chainBlocks = GetChainBlocks(nrChainBlocks);
            var badBlocks = GetBadBlocks(nrBadStored, nrChainBlocks);
            UpdateBlockProvider(chainBlocks);
            var storedBlocks = GetStoredBlocks(chainBlocks,badBlocks,nrChainBlocks,nrBadStored);
            _dbContextMock.Setup(b => b.Blocks).ReturnsDbSet(storedBlocks);
            var latestBlock = storedBlocks.Count - 1;
            _blockDataProviderServiceMock.Setup(b => b.GetHashFromHeightAsync(latestBlock)).ReturnsAsync(latestBlock.ToString());
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);

            //act
            var removedBlocks = await service.RemoveBadBlocksAsync(coinType);

            //assert
             removedBlocks.Should().BeEquivalentTo(badBlocks);
        }

        [Fact]
        public async Task GetNewBlocks_BlocksStored_NoNewBlocksInChain_ReturnEmptyList()
        {
            //arrange 
            const CoinType coinType = CoinType.BTC;
            const int nrChainBlocks = 3;
            var chainBlocks = GetChainBlocks(nrChainBlocks);
            _dbContextMock.Setup(b => b.Blocks).ReturnsDbSet(chainBlocks);
            UpdateBlockProvider(chainBlocks);
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);

            //act
            var newBlocks = await service.RemoveBadBlocksAsync(coinType);

            //assert
            newBlocks.Should().BeEmpty();
        }

        [Fact]
        public async Task GetNewBlocks_BlocksStored_NewBlocksInChain_ReturnNewBlocks()
        {
            //arrange 
            const CoinType coinType = CoinType.BTC;
            const int nrChainBlocks = 3;
            var chainBlocks = GetChainBlocks(nrChainBlocks);
            var storedBlocks = new List<Block>(chainBlocks);
            storedBlocks.RemoveRange(1,2);
            _dbContextMock.Setup(b => b.Blocks).ReturnsDbSet(storedBlocks);
            UpdateBlockProvider(chainBlocks);
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);
            var expectedResult = new List<Block> { chainBlocks[1], chainBlocks[2] };

            //act
            var newBlocks = await service.GetNewBlocksAsync(coinType);

            //assert
            newBlocks.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetNewBlocks_OnlyFirstAndLatestBlocksStored_ThreeBlocksInChain_ReturnNotStoredBlocks()
        {
            //arrange 
            const int nrChainBlocks = 3;
            var chainBlocks = GetChainBlocks(nrChainBlocks);
            var storedBlocks = new List<Block>(chainBlocks);
            storedBlocks.RemoveAt(1);
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);
            UpdateBlockProvider(chainBlocks);
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);
            var expectedResult = new List<Block> { chainBlocks[1] };

            //act
            var newBlocks = await service.GetNewBlocksAsync(CoinType.BTC);

            //assert
            newBlocks.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetNewBlocks_OnlyLatestStored_ReturnNewBlock()
        {
            // arrange
            const int nrChainBlocks = 5;
            var chainBlocks = GetChainBlocks(nrChainBlocks);
            var storedBlocks = new List<Block>(chainBlocks);
            var nonStored = storedBlocks.GetRange(0,4).ToList();
            storedBlocks.RemoveRange(0,4);
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);
            UpdateBlockProvider(chainBlocks);
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);
            var expectedResult = new List<Block>(nonStored);

            // act
            var newBlocks = await service.GetNewBlocksAsync(CoinType.BTC);

            // assert
            newBlocks.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetNewBlocks_OnlyFirstStored_ReturnNewBlock()
        {
            // arrange
            const int nrChainBlocks = 5;
            var chainBlocks = GetChainBlocks(nrChainBlocks);
            var storedBlocks = new List<Block>(chainBlocks);
            var nonStored = storedBlocks.GetRange(1, 4).ToList();
            storedBlocks.RemoveRange(1, 4);
            _dbContextMock.Setup(x => x.Blocks).ReturnsDbSet(storedBlocks);
            UpdateBlockProvider(chainBlocks);
            var service = new ExplorerNodeMonitoringService(_dbContextMock.Object, _blockDataProviderResolverMock.Object, _mockAddressRestoreService.Object);
            var expectedResult = new List<Block>(nonStored);

            // act
            var newBlocks = await service.GetNewBlocksAsync(CoinType.BTC);

            // assert
            newBlocks.Should().BeEquivalentTo(expectedResult);
        }

        private static List<Block> GetStoredBlocks(List<Block> chainBlocks, List<Block> badBlocks, int nrChainBlocks, int nrBadStored)
        {
            var storedBlocks = new List<Block>(chainBlocks);
            storedBlocks.RemoveRange((nrChainBlocks - nrBadStored), nrBadStored);
            storedBlocks.AddRange(badBlocks);
            return storedBlocks;
        }

        private void UpdateBlockProvider(List<Block> chainBlocks)
        {
            var latestHash = chainBlocks.FirstOrDefault(x => x.Height == chainBlocks.Max(b => b.Height))?.Hash;
            if (latestHash == null)
            {
                return;
            }
            _blockDataProviderServiceMock.Setup(b => b.GetBestBlockHashAsync()).ReturnsAsync(latestHash);
            foreach (var block in chainBlocks)
            {
                _blockDataProviderServiceMock.Setup(b => b.GetBlockAsync(block.Hash)).ReturnsAsync(block);
                _blockDataProviderServiceMock.Setup(b => b.GetHashFromHeightAsync(block.Height)).ReturnsAsync(block.Hash);
            }
        }
        private static List<Block> GetBadBlocks(int nrBadBlocks, int nrBlocks)
        {
            var blocks = new List<Block>();
            for (var i = (nrBlocks - nrBadBlocks); i < nrBlocks ; i++)
            {
                var badBlock = new Block() { Hash = "X" + i, Height = i, NetworkType = NetworkType.BtcMainet, CoinType = CoinType.BTC, PreviousBlockHash = "X" + (i -1) };
                blocks.Add(badBlock);
            }
            return blocks;
        }

        private static List<Block> GetChainBlocks(int nrBlocks)
        {
            var storedBlocks = new List<Block>();
            for (var i = 0; i < nrBlocks; i++)
            {
                var block = new Block() { Hash = i.ToString(), CoinType = CoinType.BTC, Height = i, NetworkType = NetworkType.BtcMainet};
                storedBlocks.Add(block);
            }
            var seqBlocks = storedBlocks.Where(b => b.Height != 0);
            foreach (var block in seqBlocks)
            {
                block.PreviousBlockHash = (block.Height - 1).ToString();
            }
            return storedBlocks;
        }
    }
}
