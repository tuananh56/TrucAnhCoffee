using Coffee.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;


namespace Coffee.Controllers
{
    public class AdminProductController : Controller
    {
        // Sử dụng Singleton để quản lý DbContext
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;

        public ActionResult Index(string search)
        {
            // Dùng Include để load đầy đủ
            var products = db.Products.Include(p => p.DanhMuc).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search)).ToList();
            }

            return View(products);
        }



        // GET: AdminProduct/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.DanhMucList = db.DanhMucs.ToList();
            return View();
        }

        // POST: AdminProduct/Create
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create(Product product, HttpPostedFileBase imageFile)
         {
             ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");

             if (ModelState.IsValid)
             {
                 string imageUrl = "/Images/default.jpg"; // Đường dẫn mặc định

                 // Xử lý ảnh upload
                 if (imageFile != null && imageFile.ContentLength > 0)
                 {
                     string imagesPath = Server.MapPath("~/Images/");
                     if (!Directory.Exists(imagesPath))
                     {
                         Directory.CreateDirectory(imagesPath); // Tạo thư mục nếu chưa tồn tại
                     }

                     string fileName = Path.GetFileName(imageFile.FileName);
                     string filePath = Path.Combine(imagesPath, fileName);
                     imageFile.SaveAs(filePath);
                     imageUrl = "/Images/" + fileName;
                 }

                 // Sử dụng ProductBuilder để tạo Product
                 var newProduct = new ProductBuilder()
                     .SetName(product.Name)
                     .SetDescription(product.Description)
                     .SetPrice(product.Price)
                     .SetStock(product.Stock)
                     .SetImageURL(imageUrl)
                     .SetCreatedAt(DateTime.Now)
                     .Build();

                 db.Products.Add(newProduct);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             return View(product);
         }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", product.MaDanhMuc);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.Find(product.ProductID);
                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;

                // Xử lý ảnh upload mới
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                    imageFile.SaveAs(filePath);
                    existingProduct.ImageURL = "/Images/" + fileName;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", product.MaDanhMuc);
            return View(product);
        }

        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
