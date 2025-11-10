using Coffee.Models;

namespace Coffee.Controllers
{
    internal class OrderDetails : OrderDetail
    {
        public int OrderID { get; set; }
        public int? ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}