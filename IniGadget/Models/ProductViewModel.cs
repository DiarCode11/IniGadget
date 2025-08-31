using System.ComponentModel.DataAnnotations;

namespace IniGadget.Models
{
    public class ProductViewModel
    {
        public Guid? Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nama harus diisi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Harga harus diisi")]
        [Range(1, uint.MaxValue, ErrorMessage = "Harga harus lebih dari 0")]
        public ulong? Price { get; set; }

        [Required(ErrorMessage = "Stok harus diisi")]
        [Range(1, uint.MaxValue, ErrorMessage = "Stok minimal harus 1")]
        public ulong? Stock { get; set; }

        [Required(ErrorMessage = "Deskripsi harus diisi")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kategori harus diisi")]
        public Guid? CategoryId { get; set; }

    }

    public class AddProductModel : ProductViewModel
    {
        [Required(ErrorMessage = "File Gambar harus diisi")]
        public IFormFile ImageFile { get; set; }
    }

    public class EditProductModel : ProductViewModel
    {
        public IFormFile? ImageFile { get; set; }
    }

    public class ProductCategoryList
    {
        public List<ProductModel> Products { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }

}
