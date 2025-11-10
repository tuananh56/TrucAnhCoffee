using Coffee.Models;
using System.Web.Mvc;

namespace Coffee.Controllers
{
    internal class ProductDetailViewModel 
    {
        public Product Product { get; set; }
        public object Comments { get; set; }
    }
}