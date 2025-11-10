using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class DangGiaoState : IOrderState
    {
        public void Handle(Order order)
        {
            Console.WriteLine("Đơn hàng đang trong trạng thái: Đang giao.");
            order.Status = "Dang giao";
        }
    }
}