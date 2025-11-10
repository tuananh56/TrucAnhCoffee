using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class ReportCollection : IReportCollection
    {
        private readonly List<ReportViewModel> _reports;

        public ReportCollection(CafeDBEntities db)
        {
            _reports = db.Orders
                .Where(o => o.Status == "Da giao" && o.OrderDate.HasValue)
                .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
                .Select(g => new ReportViewModel
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(o => o.TotalAmount),
                    TotalOrders = g.Count(),
                    TotalProductsSold = g.Sum(o => o.OrderDetails.Sum(d => d.Quantity))
                })
                .OrderByDescending(r => r.Year)
                .ThenByDescending(r => r.Month)
                .ToList();
        }

        public IReportIterator CreateIterator()
        {
            return new ReportIterator(_reports);
        }
    }

}