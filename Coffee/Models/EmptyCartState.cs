using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Coffee.Models
{
    public class EmptyCartState : CartState
    {
        public EmptyCartState(CartContext context) : base(context) { }

        public override void AddProduct(int productId)
        {
            // Logic để thêm sản phẩm vào giỏ hàng khi giỏ hàng trống
            var userId = context.UserId;
            context.Db.Carts.Add(new Cart
            {
                UserID = userId,
                ProductID = productId,
                Quantity = 1
            });
            context.Db.SaveChanges();
            context.SetState(new CartWithItemsState(context));  // Chuyển sang trạng thái có sản phẩm
        }

        public override void UpdateQuantity(int cartId, int quantity) { /* Giỏ hàng trống không thể cập nhật */ }
        public override void RemoveProduct(int cartId) { /* Giỏ hàng trống không có sản phẩm để xóa */ }
        public override void ViewCart() { /* Hiển thị giỏ hàng trống */ }
    }

    public class CartWithItemsState : CartState
    {
        public CartWithItemsState(CartContext context) : base(context) { }

        public override void AddProduct(int productId)
        {
            var userId = context.UserId;
            var cartItem = context.Db.Carts.FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                context.Db.Carts.Add(new Cart
                {
                    UserID = userId,
                    ProductID = productId,
                    Quantity = 1
                });
            }
            context.Db.SaveChanges();
        }

        public override void UpdateQuantity(int cartId, int quantity)
        {
            var cartItem = context.Db.Carts.Find(cartId);
            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                context.Db.SaveChanges();
            }
        }

        public override void RemoveProduct(int cartId)
        {
            var cartItem = context.Db.Carts.Find(cartId);
            if (cartItem != null)
            {
                context.Db.Carts.Remove(cartItem);
                context.Db.SaveChanges();
            }
        }

        public override void ViewCart() { /* Hiển thị giỏ hàng có sản phẩm */ }
    }

}