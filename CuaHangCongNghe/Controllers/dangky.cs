

using CuaHangCongNghe.Controllers;

namespace CuaHangCongNghe.Models.Tables
{
    public partial class dangky 
    {
      
      
        public int UserId { get; set; }
        public string NameUser { get; set; }
        public string Password { get; set; }
        public string EmailUser { get; set; }
        public string AddressUser { get; set; }
        public string PhoneUser { get; set; } 
        public DateOnly RegistrationDate { get; set; }
        public string Role { get; set; } 
    }
}
