using Coffee.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BCrypt.Net;
using System.Web.Security;
using BCrypt.Net;



namespace Coffee.Controllers
{
    public class AuthController : Controller
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;
        private readonly IAuthService _authService;

        public AuthController()
        {
            var baseAuthService = new AuthService();
            _authService = new AuthServiceDecorator(baseAuthService); // Dùng Decorator
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, HttpPostedFileBase Avatar)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!_authService.Register(model, Avatar, out string errorMessage))
            {
                ViewBag.Error = errorMessage;
                return View(model);
            }

            ViewBag.Success = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }

       

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _authService.Login(model.Username, model.Password, out string errorMessage);
            if (user == null)
            {
                ViewBag.Error = errorMessage;
                return View(model);
            }

            Session["UserID"] = user.UserID;
            Session["Username"] = user.Username;
            Session["Role"] = user.Role;
            Session["Avatar"] = string.IsNullOrEmpty(user.Avatar) ? "/Content/default-avatar.png" : user.Avatar;

            return user.Role == "Admin" ? RedirectToAction("Dashboard", "Admin") : RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Login");
        }
    }

}
