using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace QuizManager.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                 .WithRedirectUri("/") //Otan kanei Logout den iparxei tropos me to Authentication pou exw na kanei (Auth0) se sigkekrimeno page Redirect
                 .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
