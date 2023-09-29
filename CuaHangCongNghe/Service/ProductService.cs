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
        public void Create(ProductViewModel productViewModel)
        {
            if (productViewModel.File != null)
            {
                // Tạo đường dẫn thư mục và tên tệp tin
                string folderPath = @"C:\Hình ảnh\";
                string fileName = productViewModel.File.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                // Kiểm tra xem thư mục tồn tại chưa, nếu chưa thì tạo mới
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Kiểm tra xem tệp tin đã tồn tại chưa, nếu tồn tại, bạn có thể xử lý theo ý muốn
                if (File.Exists(filePath))
                {
                    // Ví dụ: Tạo tên tệp mới bằng cách thêm một số duy nhất vào tên
                    string extension = Path.GetExtension(fileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    int counter = 1;

                    while (File.Exists(filePath))
                    {
                        fileName = $"{fileNameWithoutExtension}_{counter}{extension}";
                        filePath = Path.Combine(folderPath, fileName);
                        counter++;
                    }
                }

                // Sao chép tệp tin vào đường dẫn đã tạo
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productViewModel.File.CopyTo(fileStream);
                }
            }

            productRepository.Create(productViewModel.ToProduct());
        }
    }
}
