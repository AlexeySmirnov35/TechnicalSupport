using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TechnicalSupport.DataBaseClasses;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Xceed.Words.NET; // For DocX

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPageJornul.xaml
    /// </summary>
    public partial class EditPageJornul : Page
    {
        private Request _requests = new Request();
        private ApplicationContext KonfigKc;

        public EditPageJornul(Request selectedRequests)
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            if (selectedRequests != null)
            {
                _requests = selectedRequests;
                LoadCommitMessages();
            }
            DataContext = _requests;
        }

        private void LoadCommitMessages()
        {
            _requests.CommitMessages = KonfigKc.CommitMessages.Where(cm => cm.RequestID == _requests.RequestID).ToList();
        }

        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (PdfDocument document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont font = new XFont("Times New Roman", 14);

                    gfx.DrawString("Заявка № " + _requests.RequestID, font, XBrushes.Black, new XRect(40, 30, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Дата: " + _requests.RequestDateStart, font, XBrushes.Black, new XRect(40, 50, page.Width, page.Height), XStringFormats.TopLeft);

                    gfx.DrawString("Заявитель: " + $"{_requests.Client.Firstname} {_requests.Client.Surname} {_requests.Client.Patranomic}", font, XBrushes.Black, new XRect(40, 70, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Подразделение: " + _requests.Client.Department.DepartmentName, font, XBrushes.Black, new XRect(40, 90, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Должность: " + _requests.Client.Position.PositionName, font, XBrushes.Black, new XRect(40, 110, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Кабинет: " + _requests.Client.Cabinet, font, XBrushes.Black, new XRect(40, 130, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Номер телефона: " + _requests.Client.NumberPhone, font, XBrushes.Black, new XRect(40, 150, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Примечание: " + _requests.Description, font, XBrushes.Black, new XRect(40, 170, page.Width, page.Height), XStringFormats.TopLeft);

                    gfx.DrawString("Исполнитель: " + $"{_requests.User.Firstname} {_requests.User.Surname} {_requests.User.Patranomic}", font, XBrushes.Black, new XRect(40, 190, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Подразделение: " + _requests.User.Department.DepartmentName, font, XBrushes.Black, new XRect(40, 210, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Должность: " + _requests.User.Position.PositionName, font, XBrushes.Black, new XRect(40, 230, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Кабинет: " + _requests.User.Cabinet, font, XBrushes.Black, new XRect(40, 250, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Номер телефона: " + _requests.User.NumberPhone, font, XBrushes.Black, new XRect(40, 270, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Дата исполнения: " + _requests.RequestDateFinish, font, XBrushes.Black, new XRect(40, 290, page.Width, page.Height), XStringFormats.TopLeft);

                    int yOffset = 310;
                    foreach (var commit in _requests.CommitMessages)
                    {
                        gfx.DrawString("Примечание: " + commit.CommitTextMessage, font, XBrushes.Black, new XRect(40, yOffset, page.Width, page.Height), XStringFormats.TopLeft);
                        yOffset += 20;
                    }

                    document.Save(saveFileDialog.FileName);
                }
            }
        }

        private void ExportToWord_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents (*.docx)|*.docx";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var document = DocX.Create(saveFileDialog.FileName))
                {
                    document.InsertParagraph("Заявка № " + _requests.RequestID).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Дата: " + _requests.RequestDateStart).Font("Times New Roman").FontSize(12);

                    document.InsertParagraph("Заявитель: " + $"{_requests.Client.Firstname} {_requests.Client.Surname} {_requests.Client.Patranomic}").Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Подразделение: " + _requests.Client.Department.DepartmentName).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Должность: " + _requests.Client.Position.PositionName).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Кабинет: " + _requests.Client.Cabinet).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Номер телефона: " + _requests.Client.NumberPhone).Font("Times New Roman").FontSize(12);

                    document.InsertParagraph("Примечание: " + _requests.Description).Font("Times New Roman").FontSize(12);

                    document.InsertParagraph("Исполнитель: " + $"{_requests.User.Firstname} {_requests.User.Surname} {_requests.User.Patranomic}").Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Подразделение: " + _requests.User.Department.DepartmentName).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Должность: " + _requests.User.Position.PositionName).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Кабинет: " + _requests.User.Cabinet).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Номер телефона: " + _requests.User.NumberPhone).Font("Times New Roman").FontSize(12);
                    document.InsertParagraph("Дата исполнения: " + _requests.RequestDateFinish).Font("Times New Roman").FontSize(12);

                    foreach (var commit in _requests.CommitMessages)
                    {
                        document.InsertParagraph("Примечание: " + commit.CommitTextMessage).Font("Times New Roman").FontSize(12);
                    }

                    document.Save();
                }
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
