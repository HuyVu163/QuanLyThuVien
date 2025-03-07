using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBorrowingRecordRepository
    {
        List<BorrowingRecord> GetBorrowingRecordsByReaderId(int readerId);
        List<BorrowingRecord> GetBorrowingRecordsByReaderIdAndStatus(int readerId);
        BorrowingRecord? GetBorrowingRecordsByBookId(int bookId);
        bool AddBorrowingRecord(BorrowingRecord borrowingRecord);
        void UpdateBorrowingRecord(BorrowingRecord borrowingRecord);
        void DeleteBorrowingRecord(int borrowingRecordId);
        List<BorrowingRecord> GetAllBorrowingRecords();
        BorrowingRecord? GetBorrowingRecordsByBorrowing(int id);
        void UpdateOverdueRecords();
    }
}
