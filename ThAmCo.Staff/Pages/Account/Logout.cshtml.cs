using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ThAmCo.Staff.Pages.Account {
    [Authorize]
    public class LogoutModel : PageModel {
        public async Task<IActionResult> OnGetAsync() {
            var authenticationProperties = new
            LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Page("/Index"))
                .Build();

            await HttpContext.SignOutAsync(
                Auth0Constants.AuthenticationScheme, authenticationProperties);

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Index");
        }
    }
}
