using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBookRepository
    {
        List<Book> GetBooksByCategory(string category);
        Book GetBookById(int bookId);
        List<Book> GetAllBooks();
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int bookId);
        Book GetBookByTitle(string title);
        List<Book> GetBooksByTitle(string keyword);
    }
}
