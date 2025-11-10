using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class YearlyRevenueReport : IReportStrategy
    {
        public List<ReportViewModel> GenerateReport(CafeDBEntities db)
        {
            return db.Orders
                .Where(o => o.Status == "Da giao" && o.OrderDate.HasValue)
                .GroupBy(o => o.OrderDate.Value.Year)
                .Select(g => new ReportViewModel
                {
                    Year = g.Key,
                    TotalRevenue = g.Sum(o => o.TotalAmount),
                    TotalOrders = g.Count(),
                    TotalProductsSold = g.Sum(o => o.OrderDetails.Sum(d => d.Quantity))
                })
                .OrderByDescending(r => r.Year)
                .ToList();
        }
    }

}