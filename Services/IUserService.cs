using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        User Login(string username, string password);
        string HashPassword(string password);
        User GetUserByUsername(string username);
        List<User> GetAll();
        User GetById(int id);
        void AddUser(User user);
        void Update(User user);
        void Delete(int id);
        User GetUserByReaderId(int readerId);
        void BorrowBook(int bookId, int userId);
    }
}
