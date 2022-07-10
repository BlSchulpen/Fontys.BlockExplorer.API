namespace Fontys.BlockExplorer.API.Controllers
{
    using Fontys.BlockExplorer.API.Dto;
    using Fontys.BlockExplorer.API.Dto.Response;
    using Fontys.BlockExplorer.Application.Services.BlockService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("api/[controller]")]
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;    
        }

        [HttpGet ("{hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBlock(string hash)
        {
            var command = new GetBlockCommand() { Hash = hash };
            var results = "test";
            if (results == null)
            { 
                return NotFound();  
            }
            return Ok(hash);
            /*
            var blockResult = await _blockService.GetBlockAsync(command);
            var response = new BlockResponse() { Hash = blockResult.Hash };
            return Json(response);
            */
        }
    }
}
