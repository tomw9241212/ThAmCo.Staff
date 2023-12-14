using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThAmCo.Staff.Models;
using ThAmCo.Staff.Services;
using ThAmCo.Staff.ViewModel;

namespace ThAmCo.Staff.Pages.Orders {
    public class ToDispatchModel : PageModel {
        private readonly ILogger<ToDispatchModel> _logger;
        private IOrdersService _ordersService;
        private ICustomerService _customerService;
        public List<OrderCustomerViewModel> OrdersCustomers { get; set; }
        public ToDispatchModel(ILogger<ToDispatchModel> logger, IOrdersService ordersService, ICustomerService customerService) {
            _logger = logger;
            _ordersService = ordersService;
            _customerService = customerService;
            OrdersCustomers = new();
        }
        public async Task OnGetAsync() {
            var orders = await _ordersService.GetOrdersByStatusAsync(OrderStatus.Confirmed);
            foreach (var order in orders) {
                var customer = await _customerService.GetCustomerAsync(order.CustomerId);
                OrdersCustomers.Add(new OrderCustomerViewModel {
                    OrderId = order.Id,
                    CustomerName = customer?.Name,
                    CustomerEmail = customer?.EmailAddress,
                    OrderStatus = order.Status,
                    SubmittedDate = order.SubmittedDate
                });
            }
        }

        public async Task<IActionResult> OnPostDispatchOrderAsync(int orderId) {
            var order = await _ordersService.GetOrderAsync(orderId);
            if (order == null) {
                return Page();
            }

            var orderUpdateDto = new OrderUpdateDto {
                OrderStatus = OrderStatus.Dispatched
            };

            await _ordersService.UpdateOrderStatusAsync(orderId, orderUpdateDto);

            return RedirectToPage(); // Redirect back to the same page to show updated status
        }

    }
}
