using Fontys.BlockExplorer.Application.Services.AddressService;
using Microsoft.AspNetCore.Mvc;

namespace Fontys.BlockExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{Hash}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAddress(string hash)
        {
            var results = await _addressService.GetAddressAsync(hash);
            if (results == null)
            {
                return NotFound();
            }
            return Ok(hash);
        }
    }
}