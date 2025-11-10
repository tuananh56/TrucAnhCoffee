using Coffee.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Coffee.Controllers
{
    public class UserController : Controller
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;
        // GET: User

        public ActionResult Index(string search)
        {
            var users = db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.Username.Contains(search) || u.Email.Contains(search) || u.Phone.Contains(search));
            }

            return View(users.ToList());
        }
        // Chỉnh sửa thông tin người dùng (Hiển thị form)
        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // Xử lý cập nhật người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model, HttpPostedFileBase Avatar)
        {
            var user = db.Users.Find(model.UserID);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.Role = model.Role;

            // Cập nhật ảnh đại diện nếu có
            if (Avatar != null && Avatar.ContentLength > 0)
            {
                string fileName = Path.GetFileName(Avatar.FileName);
                string path = Path.Combine(Server.MapPath("~/Uploads/Avatars/"), fileName);
                Avatar.SaveAs(path);
                user.Avatar = "/Uploads/Avatars/" + fileName;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Xóa người dùng
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*// Ban user trong 5 phút
        public ActionResult Ban(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.BanUntil = DateTime.Now.AddMinutes(5); // Cấm 5 phút
            db.SaveChanges();

            TempData["Success"] = $"Người dùng {user.Username} đã bị cấm trong 5 phút!";
            return RedirectToAction("Index");
        }

        // Bỏ ban ngay lập tức
        public ActionResult Unban(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.BanUntil = null;
            db.SaveChanges();

            TempData["Success"] = $"Người dùng {user.Username} đã được bỏ cấm!";
            return RedirectToAction("Index");
        }*/
        public ActionResult Profiles()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Nếu AvatarUrl rỗng, gán ảnh mặc định
            if (string.IsNullOrEmpty(user.Avatar))
            {
                user.Avatar = "/images/avatars/default-avatar.png";
            }

            return View(user);
        }
        public ActionResult EditProfiles()
        {
            int userId = (int)Session["UserID"]; // Lấy ID của user đang đăng nhập
            var user = db.Users.Find(userId); // Tìm user trong database

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new EditProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Avatar = user.Avatar // Nếu có ảnh đại diện
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfiles(EditProfileViewModel model, HttpPostedFileBase avatarFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = (int)Session["UserID"];
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Cập nhật thông tin user
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Address = model.Address;

            // Xử lý cập nhật ảnh đại diện
            if (avatarFile != null && avatarFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(avatarFile.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                avatarFile.SaveAs(filePath);
                user.Avatar = "/Content/Images/" + fileName;
            }

            db.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profiles");
        }
        // GET: User/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: User/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập!";
                return RedirectToAction("Login", "Auth");
            }

            var user = db.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại!";
                return RedirectToAction("Login", "Auth");
            }

            // Kiểm tra mật khẩu cũ
            if (user.PasswordHash != model.CurrentPassword) // Cần mã hóa mật khẩu khi kiểm tra
            {
                TempData["ErrorMessage"] = "Mật khẩu hiện tại không đúng!";
                return View(model);
            }

            // Cập nhật mật khẩu mới
            user.PasswordHash = model.NewPassword; // Cần mã hóa mật khẩu trước khi lưu
            db.SaveChanges();

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            return RedirectToAction("Profiles", "User");
        }
    }
}