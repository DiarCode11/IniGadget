using IniGadget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IniGadget.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/product")]
        public async Task<IActionResult> Create(AddProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Cek apakah category ID ada di database
            var IsCategoryExist = await _context.Category.AnyAsync(c => c.Id == product.CategoryId);
            if (!IsCategoryExist)
            {
                ModelState.AddModelError("CategoryId", "Kategori tidak ditemukan");
                return BadRequest(ModelState);
            }

            var acceptedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExt = Path.GetExtension(product.ImageFile.FileName).ToLower();
            if (!acceptedExt.Contains(fileExt))
            {
                ModelState.AddModelError("ImageFile", "File harus berupa gambar");
                return BadRequest(ModelState);
            }

            long maxSize = 2 * 1024 * 1024; // Maksimal 2 MB
            if (product.ImageFile.Length > maxSize)
            {
                ModelState.AddModelError("ImageFile", "Ukuran file tidak boleh lebih dari 2 MB");
            }

            var newFileName = Guid.NewGuid().ToString() + fileExt;
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var imagePath = Path.Combine(uploadPath, newFileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(stream);
            }

            ProductModel newProduct = new ProductModel
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Stock = product.Stock,
                ImagePath = newFileName,
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return Ok( new {message = "berhasil menambahkan data"});
        }

        [HttpPut("/product/{Id}")]
        public async Task<IActionResult> Update(Guid Id, [FromForm]EditProductModel product)
        {
            var productSelected = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
            if (productSelected == null)
            {
                return NotFound(new { message = "Data tidak ditemukan" });
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Cek apakah category ID ada di database
            var IsCategoryExist = await _context.Category.AnyAsync(c => c.Id == product.CategoryId);
            if (!IsCategoryExist)
            {
                ModelState.AddModelError("CategoryId", "Kategori tidak ditemukan");
                return BadRequest(ModelState);
            }

            if (product.ImageFile != null)
            {
                var acceptedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExt = Path.GetExtension(product.ImageFile.FileName).ToLower();
                if (!acceptedExt.Contains(fileExt))
                {
                    ModelState.AddModelError("ImageFile", "File harus berupa gambar");
                    return BadRequest(ModelState);
                }

                long maxSize = 2 * 1024 * 1024; // Maksimal 2 MB
                if (product.ImageFile.Length > maxSize)
                {
                    ModelState.AddModelError("ImageFile", "Ukuran file tidak boleh lebih dari 2 MB");
                }

                var newFileName = Guid.NewGuid().ToString() + fileExt;
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var imagePath = Path.Combine(uploadPath, newFileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                // Hapus file lama

                var oldFilePath = Path.Combine(uploadPath, productSelected.ImagePath);
                if (System.IO.File.Exists(oldFilePath)) {
                    System.IO.File.Delete(oldFilePath);
                }

                productSelected.ImagePath = imagePath;
            }

            productSelected.Name = product.Name;
            productSelected.Price = product.Price;
            productSelected.Stock = product.Stock;
            productSelected.Description = product.Description;
            productSelected.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "berhasil mengupdate data" });
        }

        [HttpDelete("/product/{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var selectedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
            if (selectedProduct == null)
            {
                return NotFound(new { message = "Data tidak ditemukan" });
            }

            _context.Products.Remove(selectedProduct);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Data berhasil dihapus" });
        }
    }
}
