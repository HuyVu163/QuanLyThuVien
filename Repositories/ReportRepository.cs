using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportDAO reportDAO;

        public ReportRepository(ReportDAO reportDAO)
        {
            this.reportDAO = reportDAO;
        }

        public List<BookReportViewModel> GetBorrowingReport(DateTime startDate, DateTime endDate) => reportDAO.GetBorrowingReport(startDate, endDate);  
    }
}
