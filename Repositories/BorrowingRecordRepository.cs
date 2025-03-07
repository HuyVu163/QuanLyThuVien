using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BorrowingRecordRepository : IBorrowingRecordRepository
    {
        private readonly BorrowingRecordDAO _borrowingRecordDAO;

        public BorrowingRecordRepository(BorrowingRecordDAO borrowingRecordDAO)
        {
            _borrowingRecordDAO = borrowingRecordDAO;
        }

        public bool AddBorrowingRecord(BorrowingRecord borrowingRecord) => _borrowingRecordDAO.AddBorrowingRecord(borrowingRecord);
       

        public void DeleteBorrowingRecord(int borrowingRecordId) => _borrowingRecordDAO.DeleteBorrowingRecord(borrowingRecordId);


        public List<BorrowingRecord> GetAllBorrowingRecords() => _borrowingRecordDAO.GetAllBorrowingRecords();
        

        public BorrowingRecord? GetBorrowingRecordsByBookId(int bookId) => _borrowingRecordDAO.GetBorrowingRecordsByBookId(bookId);
        

        public BorrowingRecord? GetBorrowingRecordsByBorrowing(int id) => _borrowingRecordDAO.GetBorrowingRecordsByBorrowing(id);
        

        public List<BorrowingRecord> GetBorrowingRecordsByReaderId(int readerId) => _borrowingRecordDAO.GetBorrowingRecordsByReaderId(readerId);
       

        public List<BorrowingRecord> GetBorrowingRecordsByReaderIdAndStatus(int readerId) => _borrowingRecordDAO.GetBorrowingRecordsByReaderIdAndStatus(readerId);



        public void UpdateBorrowingRecord(BorrowingRecord borrowingRecord) => _borrowingRecordDAO.UpdateBorrowingRecord(borrowingRecord);



        public void UpdateOverdueRecords() => _borrowingRecordDAO.UpdateOverdueRecords();


    }
}
