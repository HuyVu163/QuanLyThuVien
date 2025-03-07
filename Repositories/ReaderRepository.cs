using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly ReaderDAO _readerDAO;

        public ReaderRepository(ReaderDAO readerDAO)
        {
            _readerDAO = readerDAO;
        }

        public void AddReader(Reader reader) => _readerDAO.AddReader(reader);
        
        public void Delete(int id) => _readerDAO.Delete(id);

        public List<Reader> GetAll() => _readerDAO.GetAll();

        public Reader? GetReaderByEmail(string email) => _readerDAO.GetReaderByEmail(email);

        public Reader? GetReaderById(int id) => _readerDAO.GetReaderById(id);

        public Reader? GetReaderByPhoneNumber(string phoneNumber) => _readerDAO.GetReaderByPhoneNumber(phoneNumber);

        public List<User> GetReadersByFullName(string keyword) => _readerDAO.GetReadersByFullName(keyword);

        public void Update(Reader reader) => _readerDAO.Update(reader);
    }
}
