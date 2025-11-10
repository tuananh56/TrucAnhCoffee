using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class ReportIterator : IReportIterator
    {
        private readonly List<ReportViewModel> _reports;
        private int _currentIndex = 0;

        public ReportIterator(List<ReportViewModel> reports)
        {
            _reports = reports;
        }

        public bool HasNext()
        {
            return _currentIndex < _reports.Count;
        }

        public ReportViewModel Next()
        {
            return HasNext() ? _reports[_currentIndex++] : null;
        }
    }

}