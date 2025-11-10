using Coffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace Coffee.Controllers
{
    public class CartController : Controller
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;
    
        // 1. Thêm sản phẩm vào giỏ hàng

        //nguyên mẫu
        /* [HttpPost] // Chỉ chấp nhận POST request

         public JsonResult AddToCart(int productId)
         {
             if (Session["UserID"] == null)
             {
                 return Json(new { success = false, redirectUrl = Url.Action("Login", "Account") });
             }

             int userId = (int)Session["UserID"];
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
             return Json(new { success = true });
         }*/

        private CartSubject cartSubject = new CartSubject();


        //observer
        [HttpPost]
        public JsonResult AddToCart(int productId)
        {
            if (Session["UserID"] == null)
            {
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Account") });
            }

            int userId = (int)Session["UserID"];

            // Tạo và đăng ký Observer để cập nhật giỏ hàng
            CartView cartView = new CartView(userId);
            cartSubject.Attach(cartView);

            cartSubject.AddToCart(userId, productId);

            return Json(new { success = true });
        }



        // 2. Hiển thị giỏ hàng
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = (int)Session["UserID"];
            var cartItems = db.Carts.Where(c => c.UserID == userId).ToList();
            return View(cartItems);
        }

        // 3. Cập nhật số lượng sản phẩm
        [HttpPost]
        public ActionResult UpdateCart(int cartId, int quantity)
        {
            var cartItem = db.Carts.Find(cartId);
            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // 4. Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int cartId)
        {
            var cartItem = db.Carts.Find(cartId);
            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
       
    }
}