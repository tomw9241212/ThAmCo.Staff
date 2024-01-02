using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public class FakeOrdersService : IOrdersService {
        private readonly List<OrderGetDto> _orders = new() {
            new OrderGetDto { Id = 1, CustomerId = 1, SubmittedDate = DateTime.Now, OrderDetails = new List<OrderDetail>
                    { new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05m } } },
        };
        public async Task<List<OrderGetDto>> GetOrdersAsync() {
            return await Task.FromResult(_orders);
        }

        public async Task<OrderGetDto> GetOrderAsync(int id) {
            var order = _orders.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(order);
        }

        public async Task<List<OrderGetDto>> GetOrdersByStatusAsync(OrderStatus orderStatus) {
            var orders = _orders.Where(x => x.Status == orderStatus);
            return await Task.FromResult(orders.ToList());
        }

        public async Task UpdateOrderStatusAsync(int id, OrderUpdateDto orderUpdateDto) {
            return;
        }

    }
}
