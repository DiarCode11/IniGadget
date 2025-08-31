using IniGadget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IniGadget.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/dashboard")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/dashboard/analytics")]
        public IActionResult Analytics()
        {
            return View();
        }

        [HttpGet("/dashboard/sales")]
        public IActionResult Sales()
        {
            return View();
        }

        [HttpGet("/dashboard/users")]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
            }).ToListAsync();

            Console.WriteLine("User: " + users);

            return View(users);
        }

        [HttpGet("/dashboard/categories")]
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Category.ToListAsync();
            return View(categories);
        }

        [HttpGet("/dashboard/products")]
        public async Task<IActionResult> Products()
        {
            ProductCategoryList pcList = new ProductCategoryList
            {
                Products = await _context.Products.Include(p => p.Category).ToListAsync(),
                Categories = await _context.Category.ToListAsync()
                
            };
            return View(pcList);
        }
    }
}
