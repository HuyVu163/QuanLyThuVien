using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO userDAO;

        public UserRepository(UserDAO userDAO)
        {
            this.userDAO = userDAO;
        }

        public void AddUser(User user) => userDAO.AddUser(user);
        

        public void BorrowBook(int bookId, int userId) => userDAO.BorrowBook(bookId, userId);

        public void Delete(int id) => userDAO.Delete(id);
       
        public List<User> GetAll() => userDAO.GetAll();

        public User GetById(int id) => userDAO.GetById(id);

        public User GetUserByReaderId(int readerId) => userDAO.GetUserByReaderId(readerId);

        public User GetUserByUsername(string username) => userDAO.GetUserByUsername(username);

        public string HashPassword(string password) => userDAO.HashPassword(password);

        public User Login(string username, string password) => userDAO.Login(username, password);   

        public void Update(User user) => userDAO.Update(user);
    }
}
