using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ThAmCo.Staff.Pages.Account {
    public class LoginModel : PageModel {
        public async Task OnGetAsync(string returnUrl = "/") {
            var authenticationProperties = new
                LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(returnUrl)
                    .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme,
                authenticationProperties);
        }
    }
}
