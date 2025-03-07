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
    /// Interaction logic for ManageTransaction.xaml
    /// </summary>
    public partial class ManageTransaction : Window
    {
        private readonly BorrowingRecordService _borrowingRecordService;
        private readonly ReaderService _readerService = new ReaderService(new ReaderRepository(new ReaderDAO(new LibraryManagementContext())));
        private readonly BookService _bookService = new BookService(new BookRepository(new BookDAO(new LibraryManagementContext())));
        public ManageTransaction()
        {
            InitializeComponent();
            var context = new LibraryManagementContext();
            var borrowingDAO = new BorrowingRecordDAO(context);
            var borrowingRecordRepository = new BorrowingRecordRepository(borrowingDAO);
            _borrowingRecordService = new BorrowingRecordService(borrowingRecordRepository);
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                List<BorrowingRecord> borrowingRecords = _borrowingRecordService.GetAllBorrowingRecords();
                TransactionListView.ItemsSource = borrowingRecords;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReaderTextBox.Text) ||
                string.IsNullOrWhiteSpace(BookTextBox.Text) ||
                DueDatePicker.SelectedDate == null
                )
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BorrowingRecord borrowingRecord = new BorrowingRecord
            {
                ReaderId = (int)ReaderTextBox.Tag,
                BookId = (int)BookTextBox.Tag,
                BorrowDate = DateTime.Now,
                ReturnDate = DueDatePicker.SelectedDate.Value,
                Status = "Borrowed"

            };

            try
            {
                bool result = _borrowingRecordService.AddBorrowingRecord(borrowingRecord);

                if (result)
                {
                    MessageBox.Show("Mượn sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Sách đã hết số lượng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReaderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = ReaderTextBox.Text;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var readers = _readerService.GetAll();
                var suggestions = readers.Where(r => r.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

                ReaderSuggestionList.ItemsSource = suggestions;
                ReaderSuggestionList.DisplayMemberPath = "FullName";
                ReaderSuggestionList.Visibility = suggestions.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                ReaderSuggestionList.Visibility = Visibility.Collapsed;
            }
        }

        private void ReaderSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReaderSuggestionList.SelectedItem is Reader selectedReader)
            {
                ReaderTextBox.Text = selectedReader.FullName;
                ReaderTextBox.Tag = selectedReader.ReaderId;
                ReaderSuggestionList.Visibility = Visibility.Collapsed;
            }
        }


        private void BookTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = BookTextBox.Text;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var books = _bookService.GetAllBooks();
                var suggestions = books.Where(b => b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

                BookSuggestionList.ItemsSource = suggestions;
                BookSuggestionList.DisplayMemberPath = "Title";
                BookSuggestionList.Visibility = suggestions.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                BookSuggestionList.Visibility = Visibility.Collapsed;
            }
        }

        private void BookSuggestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookSuggestionList.SelectedItem is Book selectedBook)
            {
                BookTextBox.Text = selectedBook.Title;
                BookTextBox.Tag = selectedBook.BookId; // Lưu ID
                BookSuggestionList.Visibility = Visibility.Collapsed;
            }
        }



        private void ClearFields()
        {
            ReaderTextBox.Clear();
            BookTextBox.Clear();
            DueDatePicker.SelectedDate = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _borrowingRecordService.UpdateOverdueRecords();
            LoadData();
        }



        private void EditTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionListView.SelectedItem is BorrowingRecord selectedRecord)
            {
                if (ReaderTextBox.Tag != null && BookTextBox.Tag != null && DueDatePicker.SelectedDate != null)
                {
                    selectedRecord.ReaderId = (int)ReaderTextBox.Tag;
                    selectedRecord.BookId = (int)BookTextBox.Tag;
                    selectedRecord.ReturnDate = DueDatePicker.SelectedDate.Value;
                    try
                    {
                        _borrowingRecordService.UpdateBorrowingRecord(selectedRecord);
                        MessageBox.Show("Cập nhật giao dịch thành công!");
                        LoadData();
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi cập nhật!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn giao dịch cần cập nhật!");
            }
        }


        private void CancelTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionListView.SelectedItem is BorrowingRecord selectedRecord)
            {
                if (selectedRecord.Status == "Returned")
                {
                    MessageBox.Show("Giao dịch này không thể hủy!");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy giao dịch này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {

                        selectedRecord.Status = "Returned";


                        var book = _bookService.GetBookById(selectedRecord.BookId);
                        if (book != null)
                        {
                            book.Quantity += 1;
                            _bookService.UpdateBook(book);
                        }


                        _borrowingRecordService.UpdateBorrowingRecord(selectedRecord);

                        MessageBox.Show("Hủy giao dịch thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadData();
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn giao dịch cần hủy!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Close();
        }

        private void TransactionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionListView.SelectedItem is BorrowingRecord selectedRecord)
            {
                ReaderTextBox.Text = selectedRecord.Reader.FullName;
                BookTextBox.Text = selectedRecord.Book.Title;
                DueDatePicker.SelectedDate = selectedRecord.ReturnDate;
            }
        }
    }
}
