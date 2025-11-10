using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class ChuaGiaoState : IOrderState
    {
        public void Handle(Order order)
        {
            Console.WriteLine("Đơn hàng đang trong trạng thái: Chưa giao.");
            order.Status = "Chua giao";
        }
    }
}