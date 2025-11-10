using Coffee.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coffee.Controllers
{
    /*  //[Authorize(Roles = "Admin")]
      public class AdminController : Controller
      {
          private CafeDBEntities db = new CafeDBEntities(); // Kết nối DB
          public ActionResult Dashboard()
          {
              // Kiểm tra nếu không phải Admin thì chuyển hướng về trang thông báo
              if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
              {
                  TempData["ErrorMessage"] = "Bạn không có thẩm quyền để truy cập trang này.";
                  return RedirectToAction("Unauthorized", "Home");
              }

              return View();
          }
          // Báo cáo doanh thu theo tháng (dùng Entity Framework)
      }
    */
    public class AdminController : Controller
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;

        public ActionResult Dashboard()
        {
            if (!AdminSessionHandler.IsAdmin(Session))
            {
                TempData["ErrorMessage"] = "Bạn không có thẩm quyền để truy cập trang này.";
                return RedirectToAction("Unauthorized", "Home");
            }
            return View();
        }
    }

    public static class AdminSessionHandler
    {
        public static bool IsAdmin(HttpSessionStateBase session)
        {
            return session["Role"] != null && session["Role"].ToString() == "Admin";
        }
    }
}