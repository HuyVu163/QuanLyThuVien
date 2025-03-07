using BusinessObjects.Models;
using DataAccessLayer;
using QuanLyThuVienWPF.View_Admin;
using QuanLyThuVienWPF.View_Reader;
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

namespace QuanLyThuVienWPF.View_Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly IUserService _userService;

        public Login()
        {
            InitializeComponent();
            var context = new LibraryManagementContext();
            var userDao = new UserDAO(context);
            var userRepo = new UserRepository(userDao);
            _userService = new UserService(userRepo);
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                User? user = _userService.Login(username, password);

                if (user != null)
                {
                    MessageBox.Show($"Đăng nhập thành công!\nChào {user.Username}", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (user.Role == "Admin")
                    {
                        var adminWindow = new AdminDashboard();
                        adminWindow.Show();

                        this.Close();

                    }
                    else if (user.Role == "Reader")
                    {
                        var readerWindow = new ReaderHomepage(user.UserId);
                        readerWindow.Show();

                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
