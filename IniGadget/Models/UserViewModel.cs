using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IniGadget.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Id harus diisi")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Nama harus diisi")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        [Required(ErrorMessage = "Email tidak boleh kosong")]
        public string Email { get; set; }


        [EnumDataType(typeof(UserModel.UserRole), ErrorMessage = "Role tidak valid")]
        [RegularExpression(@"\S+", ErrorMessage = "Role tidak boleh kosong")]
        public UserModel.UserRole Role { get; set; }
    }

}
