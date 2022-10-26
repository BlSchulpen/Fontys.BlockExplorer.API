using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Fontys.BlockExplorer.Application.Services.BlockService;
using Fontys.BlockExplorer.Application.Services.NodeMonitoringService;
using Fontys.BlockExplorer.Data;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Fontys.BlockExplorer.Benchmarker
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    public class NodeMonitoringBenchmarker
    {
        private readonly  INodeMonitoringService _nodeMonitoringService;
     //   private readonly IBlockDataProviderService _blockDataProviderService;

        // mock db is used thus exectiontime EF commands not meassured
        public NodeMonitoringBenchmarker()
        {
            var mockContext = new Mock<BlockExplorerContext>();
          //  mockContext.Setup(x => x.Blocks).ReturnsDbSet();
            var mockResolver = new Mock<Func<CoinType, IBlockDataProviderService>>();
            var mockBlockService = new Mock<IBlockService>();
            var logger = new Mock<ILogger<ExplorerNodeMonitoringService>>();
            _nodeMonitoringService = new ExplorerNodeMonitoringService(mockContext.Object, mockResolver.Object,
                mockBlockService.Object, logger.Object);
        }

        [Benchmark]
        public async Task BenchMarkRemoveBadBlocks()
        {
            // arrange
            const CoinType coinType = CoinType.BTC;


            // act
            await _nodeMonitoringService.RemoveBadBlocksAsync(coinType);
        }
        /*
        private List<Block> StoredBlocks(int nrStored)
        {
            var blocks = new List<Block>();
            for (var i = 0; i < nrStored; i++)
            { 
                //TODO
                var block = new Block() {CoinType = CoinType.BTC, Hash = i.ToString(), Height = i, NetworkType = NetworkType.BtcMainet, PreviousBlockHash = i.ToString(), Transactions = new List<Transaction>()};
            }
        }
        */
    }
}
