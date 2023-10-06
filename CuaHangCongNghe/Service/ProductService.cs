using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using Microsoft.AspNetCore.Hosting;
using Shop.Models;


namespace CuaHangCongNghe.Services
{
    public class ProductService
    {
        private readonly ProductRepository productRepository;
        IWebHostEnvironment appEnvironment;

        public ProductService(ProductRepository productRepository, IWebHostEnvironment appEnvironment)
        {
            this.productRepository = productRepository;
            this.appEnvironment = appEnvironment;
        }

        public List<ProductViewModel> GetAllProducts()
        {
            var products = productRepository.GetAll();
            var productsViewModel = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var productViewModel = product.ToProductViewModel();
                productsViewModel.Add(productViewModel);
            }
            return productsViewModel;
        }

        public ProductViewModel GetProduct(int id)
        {
            var product = productRepository.Get(id);
            var productViewModel = product.ToProductViewModel();
            return productViewModel;
        }
        public void UpdateProduct(int IdProduct,int quanty)
        {
            var product = productRepository.Get(IdProduct);
            product.Stockquantity -= quanty;

            productRepository.Update(product);

        }


        public void DeleteProduct(int IdProduct)
        {

        }

        public void Create(ProductViewModel productViewModel)
        {

            if (productViewModel.File != null)
            {

                string fileName = Path.GetFileName(productViewModel.File.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    productViewModel.File.CopyTo(stream);
                }

                productViewModel.ImageUrl = "images/" + fileName;
            }
            productRepository.Create(productViewModel.ToProduct());// 
        }
    }
}
