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
    public class BorrowingRecordService
    {
        private readonly BorrowingRecordRepository borrowingRecordRepository;

        public BorrowingRecordService(BorrowingRecordRepository borrowingRecordRepository)
        {
            this.borrowingRecordRepository = borrowingRecordRepository;
        }

        public bool AddBorrowingRecord(BorrowingRecord borrowingRecord) => borrowingRecordRepository.AddBorrowingRecord(borrowingRecord);


        public void DeleteBorrowingRecord(int borrowingRecordId) => borrowingRecordRepository.DeleteBorrowingRecord(borrowingRecordId);


        public List<BorrowingRecord> GetAllBorrowingRecords() => borrowingRecordRepository.GetAllBorrowingRecords();


        public BorrowingRecord? GetBorrowingRecordsByBookId(int bookId) => borrowingRecordRepository.GetBorrowingRecordsByBookId(bookId);


        public BorrowingRecord? GetBorrowingRecordsByBorrowing(int id) => borrowingRecordRepository.GetBorrowingRecordsByBorrowing(id);


        public List<BorrowingRecord> GetBorrowingRecordsByReaderId(int readerId) => borrowingRecordRepository.GetBorrowingRecordsByReaderId(readerId);


        public List<BorrowingRecord> GetBorrowingRecordsByReaderIdAndStatus(int readerId) => borrowingRecordRepository.GetBorrowingRecordsByReaderIdAndStatus(readerId);



        public void UpdateBorrowingRecord(BorrowingRecord borrowingRecord) => borrowingRecordRepository.UpdateBorrowingRecord(borrowingRecord);



        public void UpdateOverdueRecords() => borrowingRecordRepository.UpdateOverdueRecords();
    }
}
