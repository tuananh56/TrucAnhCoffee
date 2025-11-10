using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class ProductService : IProductService
    {
        private readonly CafeDBEntities _db;

        public ProductService(CafeDBEntities db)
        {
            _db = db;
        }

        public List<Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }

        public List<Comment> GetCommentsByProductId(int productId)
        {
            return _db.Comments.Where(c => c.ProductID == productId).ToList();
        }
    }



}