using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Coffee.Models
{
    public abstract class CartState
    {
        protected CartContext context;

        public CartState(CartContext context)
        {
            this.context = context;
        }

        public abstract void AddProduct(int productId);
        public abstract void UpdateQuantity(int cartId, int quantity);
        public abstract void RemoveProduct(int cartId);
        public abstract void ViewCart();
    }

}