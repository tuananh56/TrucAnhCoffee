using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class CartSubject
    {
        private List<ICartObserver> observers = new List<ICartObserver>();
        private CafeDBEntities db = new CafeDBEntities();

        // Đăng ký Observer
        public void Attach(ICartObserver observer)
        {
            observers.Add(observer);
        }

        // Xóa Observer
        public void Detach(ICartObserver observer)
        {
            observers.Remove(observer);
        }

        // Thông báo cho tất cả Observer khi có thay đổi
        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddToCart(int userId, int productId)
        {
            var cartItem = db.Carts.FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                db.Carts.Add(new Cart
                {
                    UserID = userId,
                    ProductID = productId,
                    Quantity = 1
                });
            }
            db.SaveChanges();
            Notify();
        }
    }

}