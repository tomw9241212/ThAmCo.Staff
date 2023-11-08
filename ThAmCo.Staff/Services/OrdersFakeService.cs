using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services
{
    public class OrdersFakeService : IOrdersService
    {
        public List<Order> GetOrdersAsync()
        {
            var testOrders = new List<Order>
            {
                new Order
                {
                    CustomerId = 1,
                    RequestedDate = DateTime.Now,
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Name = "Banana",
                            Price = 0.50f
                        }
                    }
                }
            };
            return testOrders;
        }
    }
}
