using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public interface IOrdersService {
        public Task<List<OrderGetDto>> GetOrdersAsync();
        public Task<OrderGetDto?> GetOrderAsync(int id);
        public Task<List<OrderGetDto>> GetOrdersByStatusAsync(OrderStatus orderStatus);
        public Task UpdateOrderStatusAsync(int id, OrderUpdateDto order);

    }
}