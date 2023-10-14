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

        public void Delete(int id)
        {
         var product =  storeContext.Products.FirstOrDefault(x => x.Id == id);


        storeContext.Products.Remove(product);
            storeContext.SaveChanges();
        }

        public Product Get(int id)
        {

            return storeContext.Products.FirstOrDefault(x => x.Id == id);
        }

      

        public List<Product> GetAll()
        {
           return storeContext.Products.AsNoTracking().ToList();
        }

        public void Update(Product product)
        {
            var product1 = storeContext.Products.FirstOrDefault(x => x.Id == product.Id);

            if (product1 != null)
            {
               
                product1.Name = product.Name;
                product1.Description = product.Description;
                product1.Price = product.Price;
                product1.Stockquantity = product.Stockquantity;
                product1.ImageUrl = product.ImageUrl;

                storeContext.SaveChanges();
            }
        }
    }
}
