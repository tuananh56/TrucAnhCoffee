using Coffee.Models;
using System;

namespace Coffee.Controllers
{
    internal class Orders : Order
    {
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}