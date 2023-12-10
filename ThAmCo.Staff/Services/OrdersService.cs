using System.Net;
using System.Net.Http.Headers;
using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public class OrdersService : IOrdersService {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        record TokenDto(string access_token, string token_type, int expires_in);

        public OrdersService(IHttpClientFactory clientFactory,
                             IConfiguration configuration) {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<OrderGetDto?> GetOrderAsync(int id) {
            var ordersClient = _clientFactory.CreateClient();
            var serviceBaseAddress = _configuration["WebServices:Orders:BaseAddress"];
            ordersClient.BaseAddress = new Uri(serviceBaseAddress);
            var response = await ordersClient.GetAsync($"api/Orders/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<OrderGetDto>();
        }

        public async Task<List<OrderGetDto>> GetOrdersAsync() {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenValues = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Orders:AuthAudience"] },
            };

            var tokenForm = new FormUrlEncodedContent(tokenValues);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenForm);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            var ordersClient = _clientFactory.CreateClient();
            var serviceBaseAddress = _configuration["WebServices:Orders:BaseAddress"];
            ordersClient.BaseAddress = new Uri(serviceBaseAddress);
            ordersClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

            var response = await ordersClient.GetAsync("api/Orders");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<OrderGetDto>>();
        }
    }
}
