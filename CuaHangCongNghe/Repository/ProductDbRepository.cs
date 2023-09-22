using CuaHangCongNghe.Models.Shop;
using Microsoft.EntityFrameworkCore;

namespace CuaHangCongNghe.Repository
{
    public class ProductDbRepository : ProductRepository
    {
        private readonly storeContext storeContext;
        public ProductDbRepository(storeContext storeContext) { 
        
        this.storeContext = storeContext;
        }
        public void Create(Product product)
        {
            storeContext.Products.Add(product);
            storeContext.SaveChanges();
        }

        public Product Get(int id)
        {
            var allProduct = GetAll();
            return allProduct.FirstOrDefault(p => p.Id == id);
        }

      

        public List<Product> GetAll()
        {
           return storeContext.Products.AsNoTracking().ToList();
        }
    }
}
