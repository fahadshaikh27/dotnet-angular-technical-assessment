using Microsoft.AspNetCore.Mvc;
using ProductWebApp.Models;
using ProductWebApp.Services;

namespace ProductWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductApiService _productApiService;

        public ProductController(ProductApiService productApiService)
        {
            _productApiService = productApiService;
        }

        // GET: /Product
        public async Task<IActionResult> Index()
        {
            var products = await _productApiService.GetProductsAsync();

            return View(products);
        }

        // GET: /Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var success = await _productApiService
                .CreateProductAsync(product);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create product.");

                return View(product);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}