using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class GiaoHangThatBaiState : IOrderState
    {
        public void Handle(Order order)
        {
            Console.WriteLine("Đơn hàng giao thất bại.");
            order.Status = "Giao hang that bai";
        }
    }
}