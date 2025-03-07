using BusinessObjects.Models;
using DataAccessLayer;
using Microsoft.Win32;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace QuanLyThuVienWPF.View_Admin
{
    /// <summary>
    /// Interaction logic for ReportBook.xaml
    /// </summary>
    public partial class ReportBook : Window
    {

        private readonly ReportService _reportService;

        public ReportBook()
        {
            InitializeComponent();
            var context = new LibraryManagementContext();
            var reportDao = new ReportDAO(context);
            var reportRepository = new ReportRepository(reportDao);
            _reportService = new ReportService(reportRepository);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var startDate = StartDatePicker.SelectedDate ?? DateTime.MinValue;
            var endDate = EndDatePicker.SelectedDate ?? DateTime.MaxValue;

            var report = _reportService.GetBorrowingReport(startDate, endDate);
            ReportListView.ItemsSource = report;

            // Lọc Top 5 sách có số lượt mượn nhiều nhất
            var top5Books = report.OrderByDescending(r => r.BorrowCount).Take(3).ToList();
            Top5BooksListView.ItemsSource = top5Books;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Close();
        }

        private void ExportToPDF_Click(object sender, RoutedEventArgs e)
        {
            if (ReportListView.Items.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Chọn nơi lưu file PDF
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "BaoCaoMuonSach.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // Đường dẫn đến file font Arial Unicode (bạn có thể thay thế bằng font khác)
                        string fontPath = @"D:\FPT\Ki 6\PRN212\PRN212\Assignment\HuyVQ\Ass02\QuanLyThuVien\QuanLyThuVien\Arial.ttf";
                        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                        // Tạo font từ BaseFont
                        iTextSharp.text.Font unicodeFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                        // Tạo đoạn văn bản với font Unicode
                        iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("BÁO CÁO TÌNH TRẠNG MƯỢN SÁCH", unicodeFont);



                        title.Alignment = Element.ALIGN_CENTER;
                        pdfDoc.Add(title);

                        pdfDoc.Add(new iTextSharp.text.Paragraph("\n"));


                        // Tạo bảng PDF
                        PdfPTable table = new PdfPTable(7) { WidthPercentage = 100 };
                        table.SetWidths(new float[] { 1.2f, 2.5f, 1.5f, 1.5f, 1.5f, 2f, 2f });

                        // Thêm tiêu đề cột
                        string[] headers = { "Mã sách", "Tên sách", "Số lượt mượn", "Số sách đã trả", "Sách đang mượn", "Sách mượn quá hạn", "Ngày mượn gần nhất" };
                        foreach (var header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, unicodeFont))
                            {
                                BackgroundColor = new BaseColor(230, 81, 0), // Màu cam đậm
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            };
                            table.AddCell(cell);
                        }

                        // Thêm dữ liệu từ ListView vào bảng PDF
                        foreach (BookReportViewModel item in ReportListView.Items)
                        {
                            table.AddCell(new PdfPCell(new Phrase(item.BookId.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase(item.Title)) { HorizontalAlignment = Element.ALIGN_LEFT });
                            table.AddCell(new PdfPCell(new Phrase(item.BorrowCount.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase(item.ReturnedBooks.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase(item.BorrowingBooks.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase(item.OverdueBooks.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase(item.LastBorrowedDate.ToString("dd/MM/yyyy"))) { HorizontalAlignment = Element.ALIGN_CENTER });
                        }

                        pdfDoc.Add(table);
                        // Thêm tiêu đề Top 5 sách
                        pdfDoc.Add(new iTextSharp.text.Paragraph("\nTop 5 sách mượn nhiều nhất\n", unicodeFont));
                        PdfPTable top5Table = new PdfPTable(3) { WidthPercentage = 100 };
                        top5Table.SetWidths(new float[] { 1.2f, 2.5f, 1.5f });

                        // Thêm tiêu đề cột cho bảng Top 5
                        string[] top5Headers = { "Mã sách", "Tên sách", "Số lượt mượn" };
                        foreach (var header in top5Headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, unicodeFont))
                            {
                                BackgroundColor = new BaseColor(41, 128, 185), // Màu xanh
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            };
                            top5Table.AddCell(cell);
                        }

                        // Thêm dữ liệu vào bảng Top 5
                        foreach (BookReportViewModel item in Top5BooksListView.Items)
                        {
                            top5Table.AddCell(new PdfPCell(new Phrase(item.BookId.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                            top5Table.AddCell(new PdfPCell(new Phrase(item.Title)) { HorizontalAlignment = Element.ALIGN_LEFT });
                            top5Table.AddCell(new PdfPCell(new Phrase(item.BorrowCount.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        }

                        pdfDoc.Add(top5Table);

                        pdfDoc.Close();
                        writer.Close();
                    }

                    MessageBox.Show("Xuất file PDF thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}