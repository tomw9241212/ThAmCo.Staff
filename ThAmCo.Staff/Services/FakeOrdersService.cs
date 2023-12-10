using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public class FakeOrdersService : IOrdersService {
        private readonly List<OrderGetDto> _orders = new() {
            new OrderGetDto { Id = 1, CustomerId = 1, SubmittedDate = DateTime.Now, OrderDetails = new List<OrderDetail>
                    { new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f } } },
        };
        public async Task<List<OrderGetDto>> GetOrdersAsync() {
            return await Task.FromResult(_orders);
        }

        public async Task<OrderGetDto> GetOrderAsync(int id) {
            var order = _orders.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(order);
        }

    }
}
