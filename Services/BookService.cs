using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly BookRepository bookRepository;

        public BookService(BookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public void AddBook(Book book) => bookRepository.AddBook(book);


        public void DeleteBook(int bookId) => bookRepository.DeleteBook(bookId);


        public List<Book> GetAllBooks() => bookRepository.GetAllBooks();


        public Book GetBookById(int bookId) => bookRepository.GetBookById(bookId);


        public Book GetBookByTitle(string title) => bookRepository.GetBookByTitle(title);


        public List<Book> GetBooksByCategory(string category) => bookRepository.GetBooksByCategory(category);

        public List<Book> GetBooksByTitle(string keyword) => bookRepository.GetBooksByTitle(keyword);

        public void UpdateBook(Book book) => bookRepository.UpdateBook(book);
    }
}
