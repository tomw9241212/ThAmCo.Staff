using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public interface IOrdersService {
        public Task<List<OrderGetDto>> GetOrdersAsync();
        public Task<OrderGetDto> GetOrderAsync(int id);

    }
}