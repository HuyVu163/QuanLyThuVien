using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BookDAO
    {
        private readonly LibraryManagementContext _context;
        public BookDAO(LibraryManagementContext context)
        {
            _context = context;
        }

        public List<Book> GetBooksByCategory(string category)
        {
            return _context.Books.Where(b => b.Category == category).ToList();
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.Find(bookId);
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public Book GetBookByTitle(string title)
        {
            return _context.Books.FirstOrDefault(e => e.Title == title);
        }

        public List<Book> GetBooksByTitle(string keyword)
        {
            return _context.Books
                .Where(r => r.Title.ToLower().Contains(keyword.ToLower()))
                .ToList();
        }
    }
}
