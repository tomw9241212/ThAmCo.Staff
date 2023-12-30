using Microsoft.AspNetCore.Mvc.RazorPages;
using ThAmCo.Staff.Services;
using ThAmCo.Staff.ViewModel;

namespace ThAmCo.Staff.Pages.Orders {
    public class DetailsModel : PageModel {
        private readonly ILogger<DetailsModel> _logger;
        private IOrdersService _ordersService;
        private ICustomerService _customerService;
        public OrderDetailViewModel Order { get; set; } = new OrderDetailViewModel();
        public DetailsModel(ILogger<DetailsModel> logger, IOrdersService ordersService, ICustomerService customerService) {
            _logger = logger;
            _ordersService = ordersService;
            _customerService = customerService;
        }
        public async Task OnGetAsync(int id) {
            var order = await _ordersService.GetOrderAsync(id);
            if (order == null) {
                return;
            }
            var customer = await _customerService.GetCustomerAsync(order.CustomerId);
            Order = new OrderDetailViewModel {
                OrderId = order.Id,
                CustomerName = customer?.Name ?? "",
                CustomerEmail = customer?.EmailAddress ?? "",
                OrderStatus = order.Status,
                SubmittedDate = order.SubmittedDate,
                UpdatedDate = order.UpdatedDate,
                OrderDetails = order.OrderDetails
            };
        }
    }
}
