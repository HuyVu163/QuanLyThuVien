using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class BorrowingRecordDAO
    {
        private readonly LibraryManagementContext _context;

        public BorrowingRecordDAO(LibraryManagementContext context)
        {
            _context = context;
        }

        public List<BorrowingRecord> GetBorrowingRecordsByReaderId(int readerId)
        {
            return _context.BorrowingRecords.Where(e => e.ReaderId == readerId).ToList();
        }

        public List<BorrowingRecord> GetBorrowingRecordsByReaderIdAndStatus(int readerId)
        {
            return _context.BorrowingRecords.Where(e => e.ReaderId == readerId && e.Status != "Returned").ToList();
        }

        public BorrowingRecord? GetBorrowingRecordsByBookId(int bookId)
        {
            return _context.BorrowingRecords.FirstOrDefault(e => e.BookId == bookId);
        }

        public bool AddBorrowingRecord(BorrowingRecord borrowingRecord)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == borrowingRecord.BookId);

            if (book != null && book.Quantity > 0)
            {
                book.Quantity -= 1;
                _context.BorrowingRecords.Add(borrowingRecord);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateBorrowingRecord(BorrowingRecord borrowingRecord)
        {
            _context.BorrowingRecords.Update(borrowingRecord);
            _context.SaveChanges();
        }

        public void DeleteBorrowingRecord(int borrowingRecordId)
        {
            var borrowingRecord = _context.BorrowingRecords.Find(borrowingRecordId);
            if (borrowingRecord != null)
            {
                _context.BorrowingRecords.Remove(borrowingRecord);
                _context.SaveChanges();
            }
        }

        public List<BorrowingRecord> GetAllBorrowingRecords()
        {
            return _context.BorrowingRecords
                .Include(b => b.Reader) // Load thông tin độc giả
                .Include(b => b.Book)   // Load thông tin sách
                .ToList();
        }

        public BorrowingRecord? GetBorrowingRecordsByBorrowing(int id)
        {
            return _context.BorrowingRecords.FirstOrDefault(b => b.RecordId == id);
        }

        public void UpdateOverdueRecords()
        {
            var overdueRecords = _context.BorrowingRecords
                .Where(r => r.Status == "Borrowed" && r.BorrowDate.AddDays(14) < DateTime.Now)
                .ToList();

            foreach (var record in overdueRecords)
            {
                record.Status = "Overdue";
            }

            if (overdueRecords.Any())
            {
                _context.SaveChanges();
            }
        }
    }
}