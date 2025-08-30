using IniGadget.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IniGadget.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [Route("/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [Route("/login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthModel auth)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == auth.Email);

                    if (user != null && BCrypt.Net.BCrypt.Verify(auth.Password, user.Password))
                    {
                        // Tambahkan claim untuk user login
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                        var claimsPricipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync("Cookies", claimsPricipal);


                        return Redirect("/dashboard");
                    }
                } catch
                {
                    return Redirect("/server_error");
                }
            }

            // Login gagal
            Console.WriteLine("Password salah");
            ModelState.AddModelError("Email", "Email atau password salah.");
            return View(auth);
        }

        [Route("/register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Hashing password
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    // Tambahkan claim untuk user login
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPricipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync("Cookies", claimsPricipal);


                    return Redirect("/");

                } catch (DbUpdateException)
                {
                    ModelState.AddModelError("Email", "Email sudah terdaftar, silahkan login");
                }
            }

            return View(user);
        }

        [Route("/logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return Redirect("/");
        }
    }
}
