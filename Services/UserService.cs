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
    public class UserService : IUserService
    {
        private readonly UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void AddUser(User user) => userRepository.AddUser(user);


        public void BorrowBook(int bookId, int userId) => userRepository.BorrowBook(bookId, userId);

        public void Delete(int id) => userRepository.Delete(id);

        public List<User> GetAll() => userRepository.GetAll();

        public User GetById(int id) => userRepository.GetById(id);

        public User GetUserByReaderId(int readerId) => userRepository.GetUserByReaderId(readerId);

        public User GetUserByUsername(string username) => userRepository.GetUserByUsername(username);

        public string HashPassword(string password) => userRepository.HashPassword(password);

        public User Login(string username, string password) => userRepository.Login(username, password);

        public void Update(User user) => userRepository.Update(user);
    }
}
