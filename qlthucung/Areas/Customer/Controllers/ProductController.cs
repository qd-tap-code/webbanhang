using Microsoft.AspNetCore.Mvc;
using qlthucung.Models;
using System.Linq;

namespace qlthucung.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 12; // Number of products to display per page
            // Retrieve a list of categories from the database
            var categories = _context.DanhMucs.ToList();

            // Retrieve products for the current page
            var products = _context.SanPhams.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Calculate the total number of products
            var totalProducts = _context.SanPhams.Count();

            // Create a view model to hold products, categories, and pagination information
            var viewModel = new ProductCategoryViewModel
            {
                Categories = categories,
                Products = products,
                PageNumber = page,
                PageSize = pageSize,
                TotalProducts = totalProducts
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult FilterProducts(int? categoryId, int page = 1)
        {
            int pageSize = 12; // Number of products to display per page

            // Retrieve a list of categories from the database
            var categories = _context.DanhMucs.ToList();

            // Filter products by the selected category
            var filteredProducts = _context.SanPhams
                .Where(p => categoryId == null || p.IdDanhmuc == categoryId)
                .ToList();

            // Calculate the total number of filtered products
            var totalProducts = filteredProducts.Count();

            // Retrieve products for the current page
            var products = filteredProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Create a view model to hold products, categories, and pagination information
            var viewModel = new ProductCategoryViewModel
            {
                Categories = categories,
                Products = products,
                PageNumber = page,
                PageSize = pageSize,
                TotalProducts = totalProducts
            };

            return View("Index", viewModel);
        }

    }
}
