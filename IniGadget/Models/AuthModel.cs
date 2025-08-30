using System.ComponentModel.DataAnnotations;

namespace IniGadget.Models
{
    public class AuthModel
    {
        [Required(ErrorMessage = "Email harus diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password harus diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
