using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class RevenueReport
    {
        public string Period { get; set; }       // Thời gian báo cáo (tháng/năm)
        public decimal TotalRevenue { get; set; } // Tổng doanh thu
    }

}