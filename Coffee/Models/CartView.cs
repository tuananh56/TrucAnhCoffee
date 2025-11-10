using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class CartView : ICartObserver
    {
        private int userId;
        private CafeDBEntities db = new CafeDBEntities();

        public CartView(int userId)
        {
            this.userId = userId;
        }

        public void Update()
        {
            var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();
            // Cập nhật lại giỏ hàng trong view
        }
    }

}