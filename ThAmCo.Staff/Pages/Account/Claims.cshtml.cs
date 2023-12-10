using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ThAmCo.Staff.Pages.Account {
    [Authorize]
    public class ClaimsModel : PageModel {

        public void OnGet() {
        }
    }
}
