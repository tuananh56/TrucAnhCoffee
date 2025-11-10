using System;

namespace Coffee.Models
{
    public class ProductBuilder
    {
        private readonly Product _product;

        public ProductBuilder()
        {
            _product = new Product();
        }

        public ProductBuilder SetName(string name)
        {
            _product.Name = name;
            return this;
        }

        public ProductBuilder SetDescription(string description)
        {
            _product.Description = description;
            return this;
        }

        public ProductBuilder SetPrice(decimal price)
        {
            _product.Price = price;
            return this;
        }

        public ProductBuilder SetStock(int stock)
        {
            _product.Stock = stock;
            return this;
        }

        public ProductBuilder SetImageURL(string imageURL)
        {
            _product.ImageURL = imageURL;
            return this;
        }

        public ProductBuilder SetCreatedAt(DateTime createdAt)
        {
            _product.CreatedAt = createdAt;
            return this;
        }

        public ProductBuilder SetDanhMuc(int maDanhMuc)
        {
            _product.MaDanhMuc = maDanhMuc;
            return this;
        }


        public Product Build()
        {
            return _product;
        }
    }
}
