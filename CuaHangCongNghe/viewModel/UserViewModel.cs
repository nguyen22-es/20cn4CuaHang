

using System.ComponentModel.DataAnnotations;

namespace CuaHangCongNghe.Models
{
    public class UserViewModel
    {
   
        public string NameUser { get; set; }
        public string EmailUser { get; set; }
        public string AddressUser { get; set; }
        public string PhoneUser { get; set; }    
        public DateTime RegistrationDate { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "mật khẩu không khớp")]
        public string PasswordConfirm { get; set; }
    }
}
