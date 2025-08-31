using System.ComponentModel.DataAnnotations;

namespace IniGadget.Models
{
    public class CategoryModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nama harus diisi")]
        public string Name { get; set; }

        public string? IconName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<ProductModel> Products { get; set; }
    }
}
