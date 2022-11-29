using AutoMapper;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Application.Services.BlockService;
using Fontys.BlockExplorer.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Fontys.BlockExplorer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;
        private readonly IMapper _mapper;

        public BlockController(IBlockService blockService, IMapper mapper)
        {
            _blockService = blockService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MockGetBlocks()
        {
            var mockResponse = new List<BlockSummaryResponse>()
            { 
                new BlockSummaryResponse(){ CoinType = CoinType.BCH, CreationDateTime = DateTime.Now }
            };
            return Ok(mockResponse);
        }


        //TODO send coin with hash
        [HttpGet("{CoinType}/{Hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBlockAsync(CoinType cointype, string hash)
        {
            var blockResult = await _blockService.GetBlockAsync(hash, cointype);
            if (blockResult == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<BlockResponse>(blockResult);
            return Ok(response);
        }

        //TODO consider indicating a limit regarding the number of items
        //TODO fix async
        [HttpGet("{Cointype}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBlocksAsync(CoinType coinType)
        {
            var responseItems = new List<BlockSummaryResponse>();
            var blocksResult = _blockService.GetBlocks(coinType);
            foreach (var blockResult in blocksResult)
            { 
                var blockResponse = _mapper.Map<BlockSummaryResponse>(blockResult);
                responseItems.Add(blockResponse);
            }
            return Ok(responseItems);
        }

        [HttpGet("{Cointype}/Latest")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetLatestBlock(CoinType coinType)
        {
            var responseItems = new List<BlockSummaryResponse>();

            return Ok(responseItems);
        }
    }
}
