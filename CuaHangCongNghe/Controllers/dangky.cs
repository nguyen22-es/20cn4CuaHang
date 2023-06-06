using CuaHangCongNghe.Models.Tables;
using System.ComponentModel.DataAnnotations;

namespace CuaHangCongNghe.Controllers
{
    public partial class dangky 
    {
      
      
      



        [Required]
        [StringLength(50)]
        [Display(Name = "User Name")] // Đặt tên hiển thị cho thuộc tính
        public string NameUser { get; set; }
      

        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string EmailUser { get; set; }

        [StringLength(100)]
        public string AddressUser { get; set; }

        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneUser { get; set; }

        public string tendangnhap { get; set; }
        public int iddangnhap { get; set; }
        public string password { get; set; }
  


    }
}
