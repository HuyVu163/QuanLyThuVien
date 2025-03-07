using BusinessObjects.Models;
using DataAccessLayer;
using QuanLyThuVienWPF.View_Login;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuanLyThuVienWPF.View_Reader
{
    /// <summary>
    /// Interaction logic for ReaderHomepage.xaml
    /// </summary>
    public partial class ReaderHomepage : Window
    {
        private readonly BookService _bookService;
        private readonly UserService _userService;
        private readonly BorrowingRecordService _borrowingRecordService;
        private readonly ReaderService _readerService;

        private int _userId;

        public ReaderHomepage(int userID)
        {
            InitializeComponent();
            _userId = userID;

            var context = new LibraryManagementContext();
            var bookDAO = new BookDAO(context);
            var userDAO = new UserDAO(context);
            var borrDAO = new BorrowingRecordDAO(context);
            var readerDAO = new ReaderDAO(context);

            var borrRepo = new BorrowingRecordRepository(borrDAO);
            var userRepository = new UserRepository(userDAO);
            var bookRepository = new BookRepository(bookDAO);
            var readerRepository = new ReaderRepository(readerDAO);

            _bookService = new BookService(bookRepository);
            _userService = new UserService(userRepository);
            _borrowingRecordService = new BorrowingRecordService(borrRepo);
            _readerService = new ReaderService(readerRepository);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Book> books = _bookService.GetAllBooks();

            BookListView.ItemsSource = books;
        }

        private void BorrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Book book)
            {
                MessageBox.Show($"Bạn muốn mượn sách: {book.Title}");

                try
                {
                    var readerID = _userService.GetById(_userId);
                    var check = _borrowingRecordService.GetBorrowingRecordsByReaderIdAndStatus(readerID.ReaderId.GetValueOrDefault());

                    if (check.Any())
                    {
                        MessageBox.Show("Bạn đã mượn sách này rồi, vui lòng trả sách để mượn tiếp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    _userService.BorrowBook(book.BookId, readerID.ReaderId.GetValueOrDefault());
                    MessageBox.Show($"Mượn sách thành công: {book.Title}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Load lại danh sách
                    Window_Loaded(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}
