using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class ProductLoggingDecorator : ProductDecoratorBase
    {
        public ProductLoggingDecorator(IProductService productService)
            : base(productService) { }

        public override List<Product> GetAllProducts()
        {
            // Ghi log khi lấy danh sách sản phẩm
            Log.Information("🔍 Lấy danh sách sản phẩm...");

            var products = base.GetAllProducts();

            // Ghi log thêm thông tin về số lượng sản phẩm lấy được
            Log.Information($"Số sản phẩm tìm thấy: {products.Count}");

            return products;
        }

        public override Product GetProductById(int id)
        {
            // Ghi log khi lấy thông tin sản phẩm theo ID
            Log.Information($"🔍 Lấy thông tin sản phẩm có ID: {id}");

            var product = base.GetProductById(id);

            if (product == null)
            {
                Log.Warning($"Không tìm thấy sản phẩm có ID: {id}");
            }
            else
            {
                Log.Information($"Sản phẩm ID {id} đã được lấy thành công.");
            }

            return product;
        }

    }
}