using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public class FakeOrdersService : IOrdersService {
        private readonly List<Order> _orders = new()
    {
            new Order { Id = 1, CustomerId = 1, RequestedDate = DateTime.Now, Products = new List<Product>
                    { new Product { Name = "Strawberry", Price = 0.50f } } },
        };
        public async Task<IEnumerable<Order>> GetOrdersAsync() {       
            return await Task.FromResult(_orders);
        }

        public async Task<Order> GetOrderAsync(int id) {
            var order = _orders.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(order);
        }

    }
}
