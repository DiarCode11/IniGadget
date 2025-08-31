using System.ComponentModel.DataAnnotations;

namespace IniGadget.Models
{
    public class ProductModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required (ErrorMessage = "Nama harus diisi")]
        public string Name { get; set; }

        [Required (ErrorMessage = "Harga harus diisi")]
        public ulong? Price { get; set; }

        [Required (ErrorMessage = "Stok harus diisi")]
        public ulong? Stock { get; set; }

        [Required(ErrorMessage = "Deskripsi harus diisi")]
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Kategori harus diisi")]
        public Guid? CategoryId { get; set; }

        public CategoryModel Category { get; set; }

    }
}
