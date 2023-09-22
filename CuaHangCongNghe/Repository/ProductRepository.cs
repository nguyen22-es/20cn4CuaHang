using CuaHangCongNghe.Models.Shop;

namespace CuaHangCongNghe.Repository
{
    public interface ProductRepository
    {
        List<Product> GetAll();

        Product Get(int id);

        void Create(Product product);
    }
}
