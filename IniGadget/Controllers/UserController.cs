using IniGadget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace IniGadget.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPut("/users/update")]
        public async Task<IActionResult> Update([FromBody] UserViewModel user)
        {
            //Console.WriteLine(user.Name);
            //Console.WriteLine(user.Email);
            //Console.WriteLine(user.Role);
            if (!ModelState.IsValid)
            {
                //Console.WriteLine(user.Name);
                //Console.WriteLine(user.Email);
                //Console.WriteLine(user.Role);
                return BadRequest(ModelState);
            }

            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existUser == null)
            {
                return NotFound(new {message = "Data tidak ditemukan" });
            }


            existUser.Name = user.Name;
            existUser.Email = user.Email;
            existUser.Role = user.Role;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Berhasil Update data" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Console.WriteLine("Masuk ke controller delete");
                Console.WriteLine(id);
                var dataToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (dataToDelete == null)
                {
                    Console.WriteLine("Data ID tidak ditemukan");
                    return NotFound();
                }

                _context.Users.Remove(dataToDelete);
                await _context.SaveChangesAsync();

                return Redirect("/dashboard/users");

            } catch (DbException)
            {
                return NotFound();
            }
        }
    }
}
