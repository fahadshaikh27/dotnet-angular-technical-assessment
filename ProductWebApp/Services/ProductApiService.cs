using System.Net.Http.Headers;
using System.Net.Http.Json;
using ProductWebApp.Models;

namespace ProductWebApp.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductApiService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddJwtToken()
        {
            var token = _httpContextAccessor
                .HttpContext?
                .Session
                .GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("JWT TOKEN IS NULL OR EMPTY");
                return;
            }

            Console.WriteLine("JWT TOKEN FOUND IN SESSION");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }

        public async Task<List<ProductViewModel>> GetProductsAsync()
        {
            try
            {
                AddJwtToken();

                var products = await _httpClient
                    .GetFromJsonAsync<List<ProductViewModel>>(
                        "api/Product");

                return products ?? new List<ProductViewModel>();
            }
            catch (HttpRequestException)
            {
                return new List<ProductViewModel>();
            }
        }

        public async Task<ProductViewModel?> GetProductAsync(int id)
        {
            try
            {
                AddJwtToken();

                var response = await _httpClient
                    .GetAsync($"api/Product/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<ProductViewModel>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> CreateProductAsync(
            ProductViewModel product)
        {
            try
            {
                AddJwtToken();

                var response = await _httpClient
                    .PostAsJsonAsync(
                        "api/Product",
                        product);

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}