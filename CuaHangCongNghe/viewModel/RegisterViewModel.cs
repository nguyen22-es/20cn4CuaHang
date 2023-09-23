using System.ComponentModel.DataAnnotations;

namespace CuaHangCongNghe.Models
{
    public class RegisterViewModel
    {
        [Required (ErrorMessage = "Name không được để trống")]
        public string nameLogin { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Trường nhập lại mật khẩu không được để trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        [DataType(DataType.Password)]
        [Display(Name = "xác nhận mật khẩu")]
        public string PasswordConfirm { get; set; }
    }
}
