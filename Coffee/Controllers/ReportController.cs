using Coffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coffee.Controllers
{
    public class ReportController : Controller
    {
        private readonly CafeDBEntities db = DbContextSingleton.Instance.DbContext;

        public ActionResult Index(string reportType = "monthly") // Mặc định báo cáo theo tháng
        {
            ReportContext context = new ReportContext();

            switch (reportType.ToLower())
            {
                case "yearly":
                    context.SetStrategy(new YearlyRevenueReport());
                    break;
                case "monthly":
                default:
                    context.SetStrategy(new MonthlyRevenueReport());
                    break;
            }

            var reports = context.GenerateReport(db);
            return View(reports);
        }
        public ActionResult TopCustomerPurchaseDay()
        {
            var data = db.Orders
                .Where(o => o.OrderDate.HasValue)
                .AsEnumerable() // Chuyển sang LINQ to Objects
                .GroupBy(o => o.OrderDate.Value.Month)
                .Select(g => new
                {
                    Date = g.Key,
                    UniqueCustomers = g.Select(o => o.UserID).Distinct().Count(),
                    TotalOrders = g.Count()
                })
                .OrderByDescending(g => g.UniqueCustomers)
                .ToList();

            return View(data);
        }


    }

}