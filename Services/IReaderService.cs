using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IReaderService
    {
        void AddReader(Reader reader);
        Reader? GetReaderById(int id);
        Reader? GetReaderByPhoneNumber(string phoneNumber);
        Reader? GetReaderByEmail(string email);
        void Update(Reader reader);
        void Delete(int id);
        List<Reader> GetAll();
        List<User> GetReadersByFullName(string keyword);
    }
}
