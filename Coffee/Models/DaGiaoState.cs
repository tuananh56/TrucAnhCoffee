using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class DaGiaoState : IOrderState
    {
        public void Handle(Order order)
        {
            Console.WriteLine("Đơn hàng đã giao thành công!");
            order.Status = "Da giao";
        }
    }
}