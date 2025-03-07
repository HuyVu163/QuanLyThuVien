using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ReportDAO
    {
        private readonly LibraryManagementContext _context;

        public ReportDAO(LibraryManagementContext context)
        {
            _context = context;
        }

        public  List<BookReportViewModel> GetBorrowingReport(DateTime startDate, DateTime endDate)
        {
            var report =  _context.BorrowingRecords
                .Where(br => br.BorrowDate >= startDate && br.BorrowDate <= endDate)
                .GroupBy(br => br.BookId)
                .Select(g => new BookReportViewModel
                {
                    BookId = g.Key,
                    Title = g.First().Book.Title,
                    BorrowCount = g.Count(),
                    ReturnedBooks = g.Count(br => br.Status == "Returned"),
                    BorrowingBooks = g.Count(br => br.Status == "Borrowed"),
                    OverdueBooks = g.Count(br => br.Status == "Overdue"),
                    LastBorrowedDate = g.Max(br => br.BorrowDate)
                })
                .ToList();

            return report;
        }
    }
}
