using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;

using System.Threading.Tasks;

namespace Coffee.Models
{
    public class GetOrdersCommand
    {
        private readonly CafeDBEntities _db;
        private string _search;
        private string _status;

        public GetOrdersCommand(CafeDBEntities db)
        {
            _db = db;
        }

        public GetOrdersCommand WithSearch(string search)
        {
            _search = search?.Trim();
            return this;
        }

        public GetOrdersCommand WithStatus(string status)
        {
            _status = status?.Trim();
            return this;
        }

        public IQueryable<Order> Execute()
        {
            var orders = _db.Orders.Include(o => o.User)
                                   .OrderByDescending(o => o.OrderDate)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(_status))
            {
                orders = orders.Where(o => o.Status.Equals(_status, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(_search))
            {
                orders = orders.Where(SearchPredicate(_search));
            }

            return orders;
        }

        private static Expression<Func<Order, bool>> SearchPredicate(string search)
        {
            return o =>
                o.OrderID.ToString().Contains(search) ||
                o.User.FullName.Contains(search) ||
                o.User.Phone.Contains(search);
        }
    }
}
