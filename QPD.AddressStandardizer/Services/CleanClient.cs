using QPD.AddressStandardizer.Exceptions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QPD.AddressStandardizer.Services
{
    public class CleanClient : ICleanClient
    {
        private const string API_URI_SECTION = "Uri";
        private const string API_KEY_SECTION = "ApiKey";
        private const string API_SECRET_SECTION = "Secret";

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CleanClient> _logger;

        public CleanClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<CleanClient> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> CleanAddress(AddressModel model)
        {
            var uri = _configuration.GetValue<string>(API_URI_SECTION)
                ?? throw new InvalidOperationException($"There are no value in \"{API_URI_SECTION}\" section");
            var apiKey = _configuration.GetValue<string>(API_KEY_SECTION)
                ?? throw new InvalidOperationException($"There are no value in \"{API_KEY_SECTION}\" section");
            var secret = _configuration.GetValue<string>(API_SECRET_SECTION)
                ?? throw new InvalidOperationException($"There are no value in \"{API_SECRET_SECTION}\" section");

            var content = SerializeAddress(model.Address);
            using var request = CreateRequest(uri, apiKey, secret, content);

            using var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                LogError(response);
                throw new ResponseException();
            }
            
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        private string SerializeAddress(string address)
        {
            var content = new List<string> { address };
            var json = JsonSerializer.Serialize(content);
            return json;
        }

        private HttpRequestMessage CreateRequest(string uri, string apiKey, string secret, string jsonContent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", apiKey);
            request.Headers.Add("X-Secret", $"{secret}");
            return request;
        }

        private void LogError(HttpResponseMessage response)
        {
            _logger.LogError("Unable to complete request.\nDaData returned {statusCode}: {reasonPhrase}", response.StatusCode, response.ReasonPhrase);
        }
    }
}
