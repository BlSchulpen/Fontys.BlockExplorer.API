namespace Fontys.BlockExplorer.API.Controllers
{
    using AutoMapper;
    using Fontys.BlockExplorer.Application.Services.AddressService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AddressController(IAddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }

        //TODO get transactions of address
        [HttpGet("{hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAddress(string hash)
        {
            var command = new GetAddressCommand() { Hash = hash };
            var results = await _addressService.GetAddressAsync(command);
            if (results == null)
            {
                return NotFound();
            } 
            return Ok(hash);
        }
    }
}