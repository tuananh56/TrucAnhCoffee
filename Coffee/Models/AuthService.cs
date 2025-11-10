using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class AuthService : IAuthService
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;

        public bool Register(RegisterViewModel model, HttpPostedFileBase Avatar, out string errorMessage)
        {
            errorMessage = null;
            if (db.Users.Any(u => u.Username == model.Username || u.Email == model.Email))
            {
                errorMessage = "Tên đăng nhập đã tồn tại!";
                return false;
            }

            string avatarPath = null;
            if (Avatar != null && Avatar.ContentLength > 0)
            {
                string folderPath = HttpContext.Current.Server.MapPath("~/Content/images/profile");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string fileName = Path.GetFileName(Avatar.FileName);
                string path = Path.Combine(folderPath, fileName);
                Avatar.SaveAs(path);

                avatarPath = "/Content/images/profile/" + fileName;
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                Phone = model.Phone,
                Address = model.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password), // Mã hóa mật khẩu
                Role = "User",
                Avatar = avatarPath
            };

            db.Users.Add(user);
            db.SaveChanges();
            return true;
        }

        public User Login(string username, string password, out string errorMessage)
        {
            errorMessage = null;
            var user = db.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                errorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return null;
            }

            // Kiểm tra mật khẩu với BCrypt
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                errorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return null;
            }
            //amdin2222
            //admin123
            return user;
        }


        public void Logout()
        {
            HttpContext.Current.Session.Clear();
        }
    }


}