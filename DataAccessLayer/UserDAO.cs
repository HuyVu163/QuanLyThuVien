using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAO
    {
        private readonly LibraryManagementContext _context;

        public UserDAO(LibraryManagementContext context)
        {
            _context = context;
        }

        public User? Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {

                return null;
            }

            string hashedPassword = HashPassword(password);

            if (user.PasswordHash != hashedPassword)
            {

                return null;
            }

            return user;
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                //Chuyển chuỗi mật khẩu thành một mảng byte[].
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                //Tính toán mã băm từ mảng byte.
                byte[] hashBytes = sha256.ComputeHash(bytes);
                //Chuyển byte[] thành chuỗi hex (abcdef123456...).
                //Loại bỏ dấu - để có chuỗi liên tục.
                //Chuyển thành chữ thường(ToLower()).
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _context.Users
                                 .Include(u => u.Reader)
                                 .Where(u => u.Role == "Reader")
                                 .ToList();
        }


        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }


        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User? GetUserByReaderId(int readerId)
        {
            return _context.Users.FirstOrDefault(u => u.ReaderId == readerId);
        }

        public void BorrowBook(int bookId, int userId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null || book.Quantity <= 0)
            {
                throw new Exception("Không thể mượn sách!");
            }
            var borrow = new BorrowingRecord
            {
                ReaderId = userId,
                BookId = bookId,
                BorrowDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(14),
                Status = "Borrowed"
            };

            book.Quantity--;

            _context.BorrowingRecords.Add(borrow);
            _context.SaveChanges();
        }
    }
}
