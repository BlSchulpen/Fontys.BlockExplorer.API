using AutoMapper;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Application.Services.TxService;
using Fontys.BlockExplorer.Domain.CQS;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fontys.BlockExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TxController : ControllerBase
    {
        private readonly ITxService _txService;
        private readonly IMapper _mapper;

        public TxController(ITxService txService, IMapper mapper)
        {
            _txService = txService;
            _mapper = mapper;
        }

        [HttpGet("{hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTransaction(string hash)
        {
            var command = new GetTxCommand() { Hash = hash };
            var result = await _txService.GetTransactionAsync(command);
            if (result == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<Transaction,TransactionResponse>(result);
            return Ok(response);
        }
    }
}
