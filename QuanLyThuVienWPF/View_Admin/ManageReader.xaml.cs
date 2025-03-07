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
    /// Interaction logic for ManageReader.xaml
    /// </summary>
    public partial class ManageReader : Window
    {
        private readonly UserService _userService;
        private readonly ReaderService _readerService;
        private readonly BorrowingRecordService _borrowingSevice;
        public ManageReader()
        {
            InitializeComponent();
            var context = new LibraryManagementContext();
            var userDao = new UserDAO(context);
            var readerDao = new ReaderDAO(context);
            var borrowingDAO = new BorrowingRecordDAO(context);
            var userRepo = new UserRepository(userDao);
            var borrowingRepo = new BorrowingRecordRepository(borrowingDAO);
            var readerRepo = new ReaderRepository(readerDao);
            _userService = new UserService(userRepo);
            _readerService = new ReaderService(readerRepo);
            _borrowingSevice = new BorrowingRecordService(borrowingRepo);


            LoadReaders();
        }
        private void LoadReaders()
        {
            try
            {
                List<User> readers = _userService.GetAll();
                ReaderListView.ItemsSource = readers;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải độc giả: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddReader_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra họ tên chỉ chứa chữ cái và khoảng trắng
            if (!System.Text.RegularExpressions.Regex.IsMatch(FullNameTextBox.Text, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                MessageBox.Show("Họ tên chỉ được chứa chữ cái và khoảng trắng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra định dạng email
            if (!System.Text.RegularExpressions.Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra số điện thoại (phải có 10 hoặc 11 chữ số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(PhoneNumberTextBox.Text, @"^\d{10,11}$"))
            {
                MessageBox.Show("Số điện thoại phải có 10 hoặc 11 chữ số!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra tên đăng nhập không chứa khoảng trắng
            if (UsernameTextBox.Text.Contains(" "))
            {
                MessageBox.Show("Tên đăng nhập không được chứa khoảng trắng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra số điện thoại có bị trùng không
            var existingReader = _readerService.GetReaderByPhoneNumber(PhoneNumberTextBox.Text);
            if (existingReader != null)
            {
                MessageBox.Show("Số điện thoại đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra email có bị trùng không
            existingReader = _readerService.GetReaderByEmail(EmailTextBox.Text);
            if (existingReader != null)
            {
                MessageBox.Show("Email đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra tên đăng nhập có bị trùng không
            var existingUserName = _userService.GetUserByUsername(UsernameTextBox.Text);
            if (existingUserName != null)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newReader = new Reader()
            {
                FullName = FullNameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                PhoneNumber = PhoneNumberTextBox.Text.Trim()
            };

            try
            {
                _readerService.AddReader(newReader);
                int readerId = newReader.ReaderId;

                var newUser = new User()
                {
                    Username = UsernameTextBox.Text.Trim(),
                    Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString(),
                    PasswordHash = _userService.HashPassword("123"),
                    ReaderId = readerId
                };

                // Thêm User
                _userService.AddUser(newUser);

                MessageBox.Show("Thêm độc giả thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadReaders();

                // Xóa dữ liệu nhập sau khi thêm thành công
                FullNameTextBox.Clear();
                EmailTextBox.Clear();
                PhoneNumberTextBox.Clear();
                UsernameTextBox.Clear();
                RoleComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.InnerException?.Message ?? ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void EditReader_Click(object sender, RoutedEventArgs e)
        {
            int ReaderId = 0;
            if (ReaderListView.SelectedItem is User selectedReader && selectedReader.ReaderId > 0)
            {
                ReaderId = selectedReader.ReaderId.GetValueOrDefault();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn độc giả để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var reader = _readerService.GetReaderById(ReaderId);
                if (reader == null)
                {
                    MessageBox.Show("Không tìm thấy độc giả!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingPhone = _readerService.GetReaderByPhoneNumber(PhoneNumberTextBox.Text);
                if (existingPhone != null && existingPhone.ReaderId != ReaderId)
                {
                    MessageBox.Show("Số điện thoại đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existingEmail = _readerService.GetReaderByEmail(EmailTextBox.Text);
                if (existingEmail != null && existingEmail.ReaderId != ReaderId)
                {
                    MessageBox.Show("Email đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                reader.FullName = FullNameTextBox.Text;
                reader.Email = EmailTextBox.Text;
                reader.PhoneNumber = PhoneNumberTextBox.Text;
                _readerService.Update(reader);

                var user = _userService.GetUserByReaderId(reader.ReaderId);
                if (user != null)
                {
                    user.Username = UsernameTextBox.Text;
                    user.Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();
                    _userService.Update(user);
                }

                MessageBox.Show("Cập nhật độc giả thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);


                LoadReaders();
                ResetFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetFields()
        {
            FullNameTextBox.Clear();
            EmailTextBox.Clear();
            PhoneNumberTextBox.Clear();
            UsernameTextBox.Clear();
            RoleComboBox.SelectedIndex = -1;
        }

        private void DeleteReader_Click(object sender, RoutedEventArgs e)
        {
            if (ReaderListView.SelectedItem is User selectedReader)
            {

                var borrowingRecords = _borrowingSevice.GetBorrowingRecordsByReaderId((int)selectedReader.ReaderId);
                if (borrowingRecords.Any())
                {
                    MessageBox.Show("Không thể xóa độc giả vì vẫn đang mượn sách!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                MessageBoxResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa độc giả này không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var user = _userService.GetUserByReaderId(selectedReader.ReaderId.GetValueOrDefault());
                        if (user != null)
                        {
                            _userService.Delete(user.UserId);
                        }
                        _readerService.Delete(selectedReader.ReaderId.GetValueOrDefault());

                        MessageBox.Show("Xóa độc giả thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);


                        LoadReaders();
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


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Close();
        }

        private void ReaderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReaderListView.SelectedItem is User selectedUser)
            {
                FullNameTextBox.Text = selectedUser.Reader.FullName;
                EmailTextBox.Text = selectedUser.Reader.Email;
                PhoneNumberTextBox.Text = selectedUser.Reader.PhoneNumber;
                UsernameTextBox.Text = selectedUser.Username;


                RoleComboBox.SelectedItem = RoleComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedUser.Role);
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadReaders();
                return;
            }

            var filteredReaders = _readerService.GetReadersByFullName(keyword);

            ReaderListView.ItemsSource = filteredReaders;

            if (!filteredReaders.Any())
            {
                MessageBox.Show("Không tìm thấy độc giả nào!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
