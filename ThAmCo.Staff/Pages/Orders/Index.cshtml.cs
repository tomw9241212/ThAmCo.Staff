using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThAmCo.Staff.Services;
using ThAmCo.Staff.ViewModel;

namespace ThAmCo.Staff.Pages.Orders {
    [Authorize]
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private IOrdersService _ordersService;
        private ICustomerService _customerService;
        public List<OrderCustomerViewModel> Orders { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IOrdersService ordersService, ICustomerService customerService) {
            _logger = logger;
            _ordersService = ordersService;
            _customerService = customerService;
        }

        public async Task OnGetAsync() {
            var orders = await _ordersService.GetOrdersAsync();
            Orders = new();
            foreach (var order in orders) {
                var customer = await _customerService.GetCustomerAsync(order.CustomerId);
                Orders.Add(new OrderCustomerViewModel {
                    OrderId = order.Id,
                    CustomerName = customer?.Name,
                    CustomerEmail = customer?.EmailAddress,
                    OrderStatus = order.Status,
                    SubmittedDate = order.SubmittedDate
                });
            }
        }
    }
}
