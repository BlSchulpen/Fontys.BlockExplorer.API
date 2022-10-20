namespace Fontys.BlockExplorer.API.Controllers
{
    using AutoMapper;
    using Fontys.BlockExplorer.API.Dto.Response;
    using Fontys.BlockExplorer.Application.Services.BlockService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Microsoft.AspNetCore.Mvc;


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

        [HttpGet("{Hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBlockAsync(string hash)
        {
            var command = new GetBlockCommand() { Hash = hash };
            var blockResult = await _blockService.GetBlockAsync(command);
            if (blockResult == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<BlockResponse>(blockResult);
            return Ok(response);
        }
    }
}
