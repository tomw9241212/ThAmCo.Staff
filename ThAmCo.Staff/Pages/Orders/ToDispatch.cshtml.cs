using Microsoft.AspNetCore.Mvc.RazorPages;
using ThAmCo.Staff.Models;
using ThAmCo.Staff.Services;

namespace ThAmCo.Staff.Pages.Orders {
    public class ToDispatchModel : PageModel {
        private readonly ILogger<ToDispatchModel> _logger;
        private IOrdersService _ordersService;
        public List<OrderGetDto> ordersToDispatch = new();
        public ToDispatchModel(ILogger<ToDispatchModel> logger, IOrdersService ordersService) {
            _logger = logger;
            _ordersService = ordersService;
        }
        public async Task OnGetAsync() {
            ordersToDispatch = await _ordersService.GetOrdersByStatusAsync(OrderStatus.Confirmed);
        }
    }
}
