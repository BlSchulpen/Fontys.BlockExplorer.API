namespace Fontys.BlockExplorer.API.Controllers
{
    using Fontys.BlockExplorer.API.Dto;
    using Fontys.BlockExplorer.API.Dto.Response;
    using Fontys.BlockExplorer.Application.Services.BlockService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("[controller]")]
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;    
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBlock([FromBody] BlockRequest blockRequest)
        {
            var command = new GetBlockCommand() { Hash = blockRequest.Hash };
            return Json("test");
            /*
            var blockResult = await _blockService.GetBlockAsync(command);
            var response = new BlockResponse() { Hash = blockResult.Hash };
            return Json(response);
            */
        }
    }
}
