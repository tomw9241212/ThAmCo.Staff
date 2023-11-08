using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services
{
    public interface IOrdersService
    {
        public List<Order> GetOrdersAsync();
    }
}