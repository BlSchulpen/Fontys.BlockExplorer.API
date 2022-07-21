using Fontys.BlockExplorer.Application.Services.TxService;
using Fontys.BlockExplorer.Domain.CQS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fontys.BlockExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TxController : ControllerBase
    {
        private readonly ITxService _txService;

        public TxController(ITxService txService)
        {
            _txService = txService;
        }

        [HttpGet("{hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTransaction(string hash)
        {
            var command = new GetTxCommand() { Hash = hash };
            var results = _txService.GeTransactionAsync(command);
            if (results == null)
            {
                return NotFound();
            }
            return Ok(hash);
        }
    }
}
