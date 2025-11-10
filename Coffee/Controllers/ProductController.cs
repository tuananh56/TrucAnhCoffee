using Coffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace Coffee.Controllers
{
    public class ProductController : Controller
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;



        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
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
    }
}

       

       



