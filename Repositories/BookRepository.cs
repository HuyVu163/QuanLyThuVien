using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDAO bookDAO;

        public BookRepository(BookDAO bookDAO)
        {
            this.bookDAO = bookDAO;
        }

        public void AddBook(Book book) => bookDAO.AddBook(book);


        public void DeleteBook(int bookId) => bookDAO.DeleteBook(bookId);


        public List<Book> GetAllBooks() => bookDAO.GetAllBooks();


        public Book GetBookById(int bookId) => bookDAO.GetBookById(bookId);


        public Book GetBookByTitle(string title) => bookDAO.GetBookByTitle(title);


        public List<Book> GetBooksByCategory(string category) => bookDAO.GetBooksByCategory(category);

        public List<Book> GetBooksByTitle(string keyword) => bookDAO.GetBooksByTitle(keyword);

        public void UpdateBook(Book book) => bookDAO.UpdateBook(book);

    }
}
