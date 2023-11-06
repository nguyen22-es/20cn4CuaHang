using CuaHangCongNghe.Models;

namespace CuaHangCongNghe.Service
{
    public interface IProductService
    {
        List<ProductViewModel> GetAllProducts();
        void Update(ProductViewModel productViewModel);
        ProductViewModel GetProduct(int id);
        void UpdateProduct(int IdProduct, int quanty);
        void Create(ProductViewModel productViewModel);
    }
}
