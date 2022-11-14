using Fontys.BlockExplorer.Application.Services.BlockProviderService;
using Microsoft.AspNetCore.Mvc;

namespace Fontys.BlockExplorer.NodeDataManager.Workers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IBlockDataProviderService _blockDataProviderService;

        public TestController()
        {
         //   _blockDataProviderService = blockProvider;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBestAsync()
        {
            var result = await _blockDataProviderService.GetBestBlockHashAsync();
            return Ok(result);
        }
    }
}
