using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class BookReportViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int BorrowCount { get; set; }
        public int ReturnedBooks { get; set; }
        public int BorrowingBooks { get; set; }
        public int OverdueBooks { get; set; }
        public DateTime LastBorrowedDate { get; set; }
    }
}
