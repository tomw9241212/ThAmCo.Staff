using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThAmCo.Staff.Models;
using ThAmCo.Staff.Services;

namespace ThAmCo.Staff.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private OrdersFakeService _ordersFakeService;
        public List<Order> orders = new();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _ordersFakeService = new OrdersFakeService();
        }

        public void OnGet()
        {
            orders = _ordersFakeService.GetOrdersAsync();
        }
    }
}