using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class CartContext
    {
        private CartState currentState;
        public int UserId { get; set; }
        public CafeDBEntities Db { get; set; }

        public CartContext(int userId, CafeDBEntities db)
        {
            this.UserId = userId;
            this.Db = db;
            this.currentState = new EmptyCartState(this);  // Ban đầu giỏ hàng là trống
        }

        public void SetState(CartState newState)
        {
            currentState = newState;
        }

        public void AddProduct(int productId)
        {
            currentState.AddProduct(productId);
        }

        public void UpdateQuantity(int cartId, int quantity)
        {
            currentState.UpdateQuantity(cartId, quantity);
        }

        public void RemoveProduct(int cartId)
        {
            currentState.RemoveProduct(cartId);
        }

        public void ViewCart()
        {
            currentState.ViewCart();
        }
    }

}