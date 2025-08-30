using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IniGadget.Models
{
    public class UserModel
    {
        public enum UserRole
        {
            User = 0,
            Seller = 1,
            Admin = 2,
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Nama harus diisi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email harus diisi")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password harus diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        // Atribut khusus konfirmasi password
        [NotMapped]
        [Required(ErrorMessage = "Konfirmasi passowrd harus diisi")]
        [DataType(DataType.Password)]
        [Display(Name = "Konfirmasi Password")]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak sama")]
        public string ConfirmPassword { get; set; }

    }
}
