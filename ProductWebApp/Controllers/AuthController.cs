using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProductWebApp.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ProductWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var loginData = new
            {
                username = model.Username,
                password = model.Password
            };

            var json = JsonSerializer.Serialize(loginData);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var authBaseUrl = _configuration["AuthApi:BaseUrl"]
                ?? "https://localhost:7015/";

            var response = await client.PostAsync(
                new Uri(new Uri(authBaseUrl), "api/Auth/login"),
                content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid username or password.");

                return View(model);
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);

            var token = document.RootElement
                .GetProperty("token")
                .GetString();

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(
                    "",
                    "Authentication failed.");

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            HttpContext.Session.SetString("JwtToken", token);

            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
