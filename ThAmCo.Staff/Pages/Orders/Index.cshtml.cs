using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThAmCo.Staff.Models;
using ThAmCo.Staff.Services;

namespace ThAmCo.Staff.Pages.Orders {
    [Authorize]
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private IOrdersService _ordersService;
        public List<OrderGetDto> orders = new();
        public OrderGetDto order = new();

        public IndexModel(ILogger<IndexModel> logger, IOrdersService ordersService) {
            _logger = logger;
            _ordersService = ordersService;
        }

        public async Task OnGetAsync() {
            orders = await _ordersService.GetOrdersAsync();
            order = await _ordersService.GetOrderAsync(1);
        }
    }
}
