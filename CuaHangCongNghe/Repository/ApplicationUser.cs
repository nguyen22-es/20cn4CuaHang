using Microsoft.AspNetCore.Identity;

namespace CuaHangCongNghe.Repository
{
    public class ApplicationUser : IdentityUser
    {

        public string Name { get; set; }
        
        public string Address { get; set; }

        public DateTime DateTime { get; set; }


    }
}
