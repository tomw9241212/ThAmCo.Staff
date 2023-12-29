using System.Net;
using System.Net.Http.Headers;
using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public class OrdersService : IOrdersService {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private TokenDto _token;
        private DateTime _tokenExpiration;

        // Returned from the Auth0 endpoint
        record TokenDto(string access_token, string token_type, int expires_in);

        public OrdersService(IHttpClientFactory clientFactory,
                             IConfiguration configuration) {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        private async Task<string> GetOrRefreshTokenAsync() {
            // Check if token already exists this session
            if (_token != null && DateTime.UtcNow < _tokenExpiration) {
                return _token.access_token;
            }

            var tokenClient = _clientFactory.CreateClient("TokenClient");

            var tokenValues = new Dictionary<string, string> {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Orders:AuthAudience"] },
            };

            var tokenForm = new FormUrlEncodedContent(tokenValues);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenForm);
            tokenResponse.EnsureSuccessStatusCode();

            _token = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();
            // Update expiration
            _tokenExpiration = DateTime.UtcNow.AddSeconds(_token.expires_in);
            return _token.access_token;
        }

        public async Task<OrderGetDto?> GetOrderAsync(int id) {
            var ordersClient = _clientFactory.CreateClient("OrdersClient");
            var serviceBaseAddress = _configuration["WebServices:Orders:BaseAddress"];
            ordersClient.BaseAddress = new Uri(serviceBaseAddress);
            ordersClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetOrRefreshTokenAsync());
            var response = await ordersClient.GetAsync($"api/Orders/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<OrderGetDto>();
        }


        public async Task<List<OrderGetDto>> GetOrdersAsync() {

            var ordersClient = _clientFactory.CreateClient("OrdersClient");
            ordersClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetOrRefreshTokenAsync());

            var response = await ordersClient.GetAsync("api/Orders");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<OrderGetDto>>();
        }

        public async Task<List<OrderGetDto>> GetOrdersByStatusAsync(OrderStatus status) {

            var ordersClient = _clientFactory.CreateClient("OrdersClient");
            ordersClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetOrRefreshTokenAsync());

            var response = await ordersClient.GetAsync($"api/Orders?orderStatus={status}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<OrderGetDto>>();
        }

        public async Task UpdateOrderStatusAsync(int id, OrderUpdateDto orderUpdateDto) {
            var ordersClient = _clientFactory.CreateClient("OrdersClient");
            ordersClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetOrRefreshTokenAsync());

            var response = await ordersClient.PatchAsJsonAsync($"api/Orders/{id}/status", orderUpdateDto);
            response.EnsureSuccessStatusCode();

            return;
        }

    }
}
