using QPD.AddressStandardizer.Exceptions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QPD.AddressStandardizer.Services
{
    public class CleanClient : ICleanClient
    {
        private const string API_SETTINGS_SECTION = "Api";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CleanClient> _logger;

        public CleanClient(IHttpClientFactory httpClientFactory, ILogger<CleanClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> CleanAddress(AddressModel model)
        {
            var apiSettings = SettingsLoader.Load<ApiSettings>(API_SETTINGS_SECTION);

            var content = SerializeAddress(model.Address);
            using var request = CreateRequest(apiSettings, content);

            using var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            CheckForError(response);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        private string SerializeAddress(string address)
        {
            var content = new List<string> { address };
            var json = JsonSerializer.Serialize(content);
            return json;
        }

        private HttpRequestMessage CreateRequest(ApiSettings settings, string jsonContent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, settings.Uri);
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", settings.Key);
            request.Headers.Add("X-Secret", settings.Secret);
            return request;
        }

        private void CheckForError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                LogError(response);
                throw new ResponseException();
            }
        }

        private void LogError(HttpResponseMessage response)
        {
            _logger.LogError("Unable to complete request.\nDaData returned {statusCode}: {reasonPhrase}", response.StatusCode, response.ReasonPhrase);
        }
    }
}
