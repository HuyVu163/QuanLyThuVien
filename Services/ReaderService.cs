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
    public class ReaderService : IReaderService
    {
        private readonly ReaderRepository _readerRepo;

        public ReaderService(ReaderRepository readerRepo)
        {
            _readerRepo = readerRepo;
        }

        public void AddReader(Reader reader) => _readerRepo.AddReader(reader);

        public void Delete(int id) => _readerRepo.Delete(id);

        public List<Reader> GetAll() => _readerRepo.GetAll();

        public Reader? GetReaderByEmail(string email) => _readerRepo.GetReaderByEmail(email);

        public Reader? GetReaderById(int id) => _readerRepo.GetReaderById(id);

        public Reader? GetReaderByPhoneNumber(string phoneNumber) => _readerRepo.GetReaderByPhoneNumber(phoneNumber);

        public List<User> GetReadersByFullName(string keyword) => _readerRepo.GetReadersByFullName(keyword);

        public void Update(Reader reader) => _readerRepo.Update(reader);
    }
}
