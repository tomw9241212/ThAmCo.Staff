using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThAmCo.Staff.Services;

namespace ThAmCo.Staff.Pages.Customers {
    [Authorize]
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private ICustomerService _customerService;
        public List<CustomerViewModel> Customers { get; set; }
        public IndexModel(ILogger<IndexModel> logger, ICustomerService customerService) {
            _logger = logger;
            _customerService = customerService;
        }

        public async Task OnGetAsync() {
            var customers = await _customerService.GetCustomersAsync();
            Customers = new();
            foreach (var customer in customers) {
                Customers.Add(new CustomerViewModel {
                    Id = customer.Id,
                    Name = customer.Name,
                    AddressLine1 = customer.AddressLine1,
                    PostCode = customer.PostCode,
                    EmailAddress = customer.EmailAddress,
                    PhoneNumber = customer.PhoneNumber,
                    AvailableFunds = customer.AvailableFunds
                });
            }
        }

        public async Task<IActionResult> OnPostDeleteCustomerAsync(int id) {
            var deleted = await _customerService.DeleteCustomerAsync(id);

            if (deleted) {
                return RedirectToPage();
            } else {
                TempData["ErrorMessage"] = "Failed to delete customer";
                return RedirectToPage();
            }
        }

    }
}
