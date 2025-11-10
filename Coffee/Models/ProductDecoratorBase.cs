using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;



namespace Coffee.Models
{
    // Lớp cơ sở của Decorator, cung cấp các phương thức mặc định và có thể được ghi đè
    public abstract class ProductDecoratorBase : IProductService
    {
        // Đối tượng dịch vụ sản phẩm sẽ được trang trí
        protected IProductService _productService;

        // Constructor nhận vào dịch vụ sản phẩm
        public ProductDecoratorBase(IProductService productService)
        {
            _productService = productService;
        }

        // Phương thức mặc định lấy tất cả sản phẩm (có thể ghi đè)
        public virtual List<Product> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }

        // Phương thức mặc định lấy sản phẩm theo ID (có thể ghi đè)
        public virtual Product GetProductById(int id)
        {
            return _productService.GetProductById(id);
        }

        // Bạn có thể thêm các phương thức khác tại đây nếu cần, ví dụ như:
        // public virtual List<Comment> GetCommentsByProductId(int id)
        // {
        //     return _productService.GetCommentsByProductId(id);
        // }
    }

}