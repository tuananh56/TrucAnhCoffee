using System.Data.Entity;  // Đảm bảo sử dụng Entity Framework

using Coffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coffee.Controllers
{
    public class DanhMucController : Controller
    {
        // GET: DanhMuc
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;

        public ActionResult Index()
        {
            // Lấy danh sách tất cả danh mục từ cơ sở dữ liệu
            var categories = db.DanhMucs.ToList();

            // Truyền danh sách danh mục vào View
            return View(categories);
        }
        // GET: DanhMuc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DanhMuc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucs.Add(danhMuc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }
        // GET: DanhMuc/Delete/5
        public ActionResult Delete(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: DanhMuc/Details/5
        public ActionResult Details(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }
        // GET: DanhMuc/Edit/5
        public ActionResult Edit(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: DanhMuc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }

    }
}