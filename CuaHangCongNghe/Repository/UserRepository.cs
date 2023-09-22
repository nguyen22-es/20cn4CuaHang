using CuaHangCongNghe.Models.Shop;

namespace CuaHangCongNghe.Repository
{
    public interface UserRepository
    {
        User TryGetByUserId(string id);

        List<User> GetAll();

        void AddInformation(User userInfo);

        void AddProduct(string id, Product product,int quantity);

        void DeleteOder(int id,Product product,int quantity);

        void CreateUser(User user);



        
    }
}
