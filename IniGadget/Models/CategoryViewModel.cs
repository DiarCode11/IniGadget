using System.ComponentModel.DataAnnotations;

namespace IniGadget.Models
{
    public class CategoryViewModel
    {
        public Guid? id { get; set; }

        [Required(ErrorMessage = "Nama kategori harus diisi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "File gambar harus diisi")]
        public IFormFile ImageFile { get; set; }
    }
}
