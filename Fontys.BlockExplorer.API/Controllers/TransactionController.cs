using AutoMapper;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Application.Services.TxService;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fontys.BlockExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService txService, IMapper mapper)
        {
            _transactionService = txService;
            _mapper = mapper;
        }

        [HttpGet("{Cointype}/{Hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTransaction(CoinType coinType, string hash)
        {
            var result = await _transactionService.GetTransactionAsync(coinType, hash);
            if (result == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<Transaction, TransactionResponse>(result);
            return Ok(response);
        }

        [HttpGet("{Cointype}/latest")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetlatestTransaction(CoinType coinType)
        {
            var result = await _transactionService.GetLatestTransactionsAsync(coinType);
            if (result == null)
            {
                return NotFound();
            }
            var responses = new List<TransactionResponse>();
            foreach (var transaction in result)
            {
                var response = _mapper.Map<Transaction, TransactionResponse>(transaction);
                responses.Add(response);
            }
            return Ok(responses);
        }
    }
}
