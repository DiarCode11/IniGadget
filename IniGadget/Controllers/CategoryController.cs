using IniGadget.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IniGadget.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost("/category/create")]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var allowedExtentions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(model.ImageFile.FileName).ToLower();

            if (!allowedExtentions.Contains(ext))
            {
                Console.WriteLine(ext);
                Console.WriteLine(allowedExtentions);
                ModelState.AddModelError("ImageFile", "Format file harus gambar");
                return BadRequest(ModelState);
            }

            long maxSize = 2 * 1024 * 1024; // 2 MB
            if (model.ImageFile.Length > maxSize)
            {
                ModelState.AddModelError("ImageFile", "Ukuran file lebih dari 2 MB");
                return BadRequest(ModelState);
            }

            // Membuat nama file unik
            var imageName = Guid.NewGuid().ToString() + ext;

            // Membuat datetime WITA
            var witaTimezone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime witaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, witaTimezone);

            CategoryModel new_category = new CategoryModel
            {
                Name = model.Name,
                IconName = imageName,
                CreatedAt = witaNow,
                UpdatedAt = witaNow
            };

            // Buat Folder Tujuan 
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, imageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            };

            // Simpan ke database
            await _context.Category.AddAsync(new_category);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Data berhasil ditambahkan" });
        }

        [HttpPut("/category/update/{Id}")]
        public async Task<IActionResult> Update(Guid Id, [FromForm] CategoryViewModel model)
        {
            var existingData = await _context.Category.FirstOrDefaultAsync(c => c.Id == Id);
            if (existingData == null)
            {
                return NotFound(new { message = "Data tidak ditemukan" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var allowedExtentions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(model.ImageFile.FileName).ToLower();

            if (!allowedExtentions.Contains(ext))
            {
                Console.WriteLine(ext);
                Console.WriteLine(allowedExtentions);
                ModelState.AddModelError("ImageFile", "Format file harus gambar");
                return BadRequest(ModelState);
            }

            long maxSize = 2 * 1024 * 1024; // 2 MB
            if (model.ImageFile.Length > maxSize)
            {
                ModelState.AddModelError("ImageFile", "Ukuran file lebih dari 2 MB");
                return BadRequest(ModelState);
            }

            // Membuat nama file unik
            var imageName = Guid.NewGuid().ToString() + ext;

            // Membuat datetime WITA
            var witaTimezone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime witaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, witaTimezone);

            // Buat Folder Tujuan 
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, imageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }

            // Hapus file lama
            var oldFilePath = Path.Combine(uploadPath, existingData.IconName);
            if (System.IO.File.Exists(oldFilePath)) {
                System.IO.File.Delete(oldFilePath);
            }

            // Simpan ke database
            existingData.Name = model.Name;
            existingData.IconName = imageName;
            existingData.UpdatedAt = witaNow;


            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil ditambahkan" });
        }

        [HttpDelete("/category/delete/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var dataToDelete = await _context.Category.FirstOrDefaultAsync(c => c.Id == Id);
            if (dataToDelete == null)
            {
                return NotFound(new { message = "Data tidak ditemukan" });
            };

            _context.Category.Remove(dataToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Berhasil hapus data" });
        }
    }
}
