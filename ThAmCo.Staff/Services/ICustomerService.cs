using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public interface ICustomerService {
        public Task<List<CustomerGetDto>> GetCustomersAsync();
        public Task<CustomerGetDto?> GetCustomerAsync(int id);
        public Task<bool> DeleteCustomerAsync(int id);

    }
}