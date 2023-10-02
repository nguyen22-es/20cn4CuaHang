using System.ComponentModel.DataAnnotations;

namespace CuaHangCongNghe.Models
{
    public class LoginViewModel 
    {
        [Required]
   
        public string NameLogin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; } = "Home/Index";
    }
}
