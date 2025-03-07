using QuanLyThuVienWPF.View_Login;
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
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void ManageBooks_Click(object sender, RoutedEventArgs e)
        {
            ManagerBook managerBook = new ManagerBook();

            managerBook.Show();
            this.Close();
        }

        private void ManageReaders_Click(object sender, RoutedEventArgs e)
        {
            ManageReader manageReader = new ManageReader();
            manageReader.Show();
            this.Close();
        }

        private void ManageTransactions_Click(object sender, RoutedEventArgs e)
        {
            ManageTransaction manageTransaction = new ManageTransaction();
            manageTransaction.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        private void Manage_Click(object sender, RoutedEventArgs e)
        {
            ReportBook reportBook = new ReportBook();
            reportBook.Show();
            this.Close();
        }
    }
}
