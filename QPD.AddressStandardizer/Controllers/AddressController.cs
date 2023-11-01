using AutoMapper;
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
        private readonly IMapper _mapper;

        public AddressController(ICleanClient cleanClient, ILogger<AddressController> logger, IMapper mapper)
        {
            _cleanClient = cleanClient;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("clean")]
        public async Task<IActionResult> Clean(AddressCleanRequest request) 
        {
            try
            {
                LogCleanRequest(request);

                var model = _mapper.Map<AddressModel>(request);

                var result = await _cleanClient.CleanAddress(model);
                return Ok(result);
            }
            catch (ResponseException)
            {
                return BadRequest();
            }
        }

        private void LogCleanRequest(AddressCleanRequest request)
        {
            _logger.LogInformation("Requested address clean for {address} from {ip}", request.Address, HttpContext.Connection.RemoteIpAddress?.MapToIPv4());
        }
    }
}
