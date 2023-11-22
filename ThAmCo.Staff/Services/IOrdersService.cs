using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services
{
    public interface IOrdersService
    {
        public Task<IEnumerable<Order>> GetOrdersAsync();
        public Task<Order> GetOrderAsync(int id);

    }
}