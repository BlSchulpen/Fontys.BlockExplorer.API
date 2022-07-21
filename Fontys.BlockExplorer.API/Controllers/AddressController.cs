namespace Fontys.BlockExplorer.API.Controllers
{
    using Fontys.BlockExplorer.Application.Services.AddressService;
    using Fontys.BlockExplorer.Domain.CQS;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAddress(string hash)
        {
            var command = new GetAddressCommand() { Hash = hash };
            var results = _addressService.GetAddressAsync(command);
            if (results == null)
            {
                return NotFound();
            }
            return Ok(hash);
        }
    }
}