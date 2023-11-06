using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;

namespace CuaHangCongNghe.Service
{
    public interface IUserService
    {
        void AddInformation(string userId, UserViewModel userViewModel);
        List<UserViewModel> GetAll();
        UserViewModel GetUser(string id);
        User GetUserl(string id);
        void createUser(string IdUser);
    }
}
