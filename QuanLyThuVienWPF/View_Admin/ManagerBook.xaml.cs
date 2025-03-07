using BusinessObjects.Models;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyThuVienWPF.View_Admin
{
    /// <summary>
    /// Interaction logic for ManagerBook.xaml
    /// </summary>
    public partial class ManagerBook : Window
    {
        private readonly BookService _bookService;
        private readonly BorrowingRecordService _borrowingRecordService;
        public ManagerBook()
        {
            InitializeComponent();
            var context = new LibraryManagementContext();
            var bookDao = new BookDAO(context);
            var borrowingRecordDao = new BorrowingRecordDAO(context);
            var borrowingRecordRepository = new BorrowingRecordRepository(borrowingRecordDao);
            var bookRepository = new BookRepository(bookDao);
            _bookService = new BookService(bookRepository);
            _borrowingRecordService = new BorrowingRecordService(borrowingRecordRepository);
            LoadBooks();
        }


        private void LoadBooks()
        {
            try
            {
                List<Book> readers = _bookService.GetAllBooks();
                BookListView.ItemsSource = readers;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sach book: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(AuthorTextBox.Text) ||
                string.IsNullOrWhiteSpace(CategoryTextBox.Text) ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra số lượng sách có phải là số nguyên dương không
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng sách phải là một số lớn hơn 0!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra giá sách có phải là số thực dương không
            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Giá sách phải là một số thực dương!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra tác giả chỉ chứa chữ cái và khoảng trắng
            if (!System.Text.RegularExpressions.Regex.IsMatch(AuthorTextBox.Text, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                MessageBox.Show("Tên tác giả chỉ được chứa chữ cái và khoảng trắng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra tiêu đề sách có bị trùng không
            var existingBook = _bookService.GetBookByTitle(TitleTextBox.Text);
            if (existingBook != null)
            {
                MessageBox.Show("Sách đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newBook = new Book()
                {
                    Title = TitleTextBox.Text.Trim(),
                    Author = AuthorTextBox.Text.Trim(),
                    Category = CategoryTextBox.Text.Trim(),
                    Quantity = quantity,
                    Price = price
                };

                // Thêm sách vào cơ sở dữ liệu
                _bookService.AddBook(newBook);

                MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBooks();

                // Xóa dữ liệu nhập sau khi thêm thành công
                TitleTextBox.Clear();
                AuthorTextBox.Clear();
                CategoryTextBox.Clear();
                QuantityTextBox.Clear();
                PriceTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.InnerException?.Message ?? ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int BookId = 0;
            if (BookListView.SelectedItem is Book selectedBook && selectedBook.BookId > 0)
            {
                BookId = selectedBook.BookId;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sách để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(AuthorTextBox.Text) ||
                string.IsNullOrWhiteSpace(CategoryTextBox.Text) ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existingBook = _borrowingRecordService.GetBorrowingRecordsByBookId(BookId);
            if (existingBook != null)
            {
                MessageBox.Show("Sách đang được mượn không thể chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var book = _bookService.GetBookById(BookId);
            if (book == null)
            {
                MessageBox.Show("Sách không tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var existingbook = _bookService.GetBookByTitle(TitleTextBox.Text);
            if (existingbook != null && existingbook.BookId != BookId)
            {
                MessageBox.Show("Sách đã tồn tại !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                book.Title = TitleTextBox.Text;
                book.Author = AuthorTextBox.Text;
                book.Category = CategoryTextBox.Text;
                book.Quantity = int.Parse(QuantityTextBox.Text);
                book.Price = decimal.Parse(PriceTextBox.Text);
                _bookService.UpdateBook(book);
                MessageBox.Show("Chỉnh sửa sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.InnerException?.Message ?? ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookListView.SelectedItem is Book selectedReader)
            {

                var borrowingRecords = _borrowingRecordService.GetBorrowingRecordsByBookId((int)selectedReader.BookId);
                if (borrowingRecords != null)
                {
                    MessageBox.Show("Không thể xóa sách vì sách vẫn đang được mượn !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                MessageBoxResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa cuốn sách này không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Xóa User
                        var book = _bookService.GetBookById(selectedReader.BookId);
                        if (book != null)
                        {
                            _bookService.DeleteBook(book.BookId);
                        }


                        MessageBox.Show("Xóa book thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadBooks();
                        ResetFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi xảy ra: {ex.InnerException?.Message ?? ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn độc giả để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ResetFields()
        {
            TitleTextBox.Clear();
            AuthorTextBox.Clear();
            CategoryTextBox.Clear();
            QuantityTextBox.Clear();
            PriceTextBox.Clear();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadBooks();
                return;
            }

            var filteredReaders = _bookService.GetBooksByTitle(keyword);

            BookListView.ItemsSource = filteredReaders;

            if (!filteredReaders.Any())
            {
                MessageBox.Show("Không tìm thấy độc giả nào!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Close();
        }

        private void BookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookListView.SelectedItem is Book selectedBook)
            {
                TitleTextBox.Text = selectedBook.Title;
                AuthorTextBox.Text = selectedBook.Author;
                CategoryTextBox.Text = selectedBook.Category;
                QuantityTextBox.Text = selectedBook.Quantity.ToString();
                PriceTextBox.Text = selectedBook.Price.ToString();

            }

        }
    }
}
