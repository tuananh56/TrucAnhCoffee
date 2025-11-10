using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class ReportContext
    {
        private IReportStrategy _reportStrategy;

        public void SetStrategy(IReportStrategy strategy)
        {
            _reportStrategy = strategy;
        }

        public List<ReportViewModel> GenerateReport(CafeDBEntities db)
        {
            return _reportStrategy.GenerateReport(db);
        }
    }

}