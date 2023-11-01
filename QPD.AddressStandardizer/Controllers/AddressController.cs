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
        private readonly ILogger<AddressController> _logger;

        public AddressController(ICleanClient cleanClient, ILogger<AddressController> logger)
        {
            _cleanClient = cleanClient;
            _logger = logger;
        }

        [HttpGet("clean")]
        public async Task<IActionResult> Clean(string address) 
        {
            try
            {
                LogCleanRequest(address);

                var result = await _cleanClient.CleanAddress(address);
                return Ok(result);
            }
            catch (ResponseException)
            {
                return BadRequest();
            }
        }

        private void LogCleanRequest(string address)
        {
            _logger.LogInformation("Requested address clean for {address} from {ip}", address, HttpContext.Connection.RemoteIpAddress?.MapToIPv4());
        }
    }
}
