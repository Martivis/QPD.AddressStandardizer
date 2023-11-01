using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPD.AddressStandardizer.Exceptions;
using QPD.AddressStandardizer.Services;

namespace QPD.AddressStandardizer.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ICleanClient _cleanClient;

        public AddressController(ICleanClient cleanClient)
        {
            _cleanClient = cleanClient;
        }

        [HttpGet("clean")]
        public async Task<IActionResult> Clean(string address) 
        {
            try
            {
                var result = await _cleanClient.CleanAddress(address);
                return Ok(result);
            }
            catch (ResponseException)
            {
                return BadRequest();
            }
        }

    }
}
