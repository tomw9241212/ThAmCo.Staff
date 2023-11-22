using System.Runtime.CompilerServices;
using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public class OrdersService : IOrdersService {

        private readonly HttpClient _client;

        public OrdersService(HttpClient client, 
            IConfiguration configuration) {
            var url = configuration["WebServices:Orders:BaseUrl"];
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }
        public async Task<Order> GetOrderAsync(int id) {
            var response = await _client.GetAsync($"api/Orders/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync() {
            var response = await _client.GetAsync($"api/Orders");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
        }
    }
}
