using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly ReportRepository reportRepo;

        public ReportService(ReportRepository reportRepo)
        {
            this.reportRepo = reportRepo;
        }

        public List<BookReportViewModel> GetBorrowingReport(DateTime startDate, DateTime endDate) => reportRepo.GetBorrowingReport(startDate, endDate);
    }
}
