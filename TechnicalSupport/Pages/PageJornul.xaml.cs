using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.Pages
{
    public partial class PageJornul : Page
    {
        private ApplicationContext KonfigKc;
        private int currentPage = 1;
        private const int PageSize = 10;

        public PageJornul()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            LoadStatuses();
            DisplayPage();
        }

        private void LoadStatuses()
        {
            var statuses = KonfigKc.StatusRequests.ToList();

            StatusComboBox.Items.Clear();

            ComboBoxItem allItem = new ComboBoxItem
            {
                Content = "Все"
            };
            StatusComboBox.Items.Add(allItem);

            foreach (var status in statuses)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = status.StatusName
                };
                StatusComboBox.Items.Add(item);
            }

            StatusComboBox.SelectedIndex = 0;
        }

        private void DisplayPage()
        {
            var searchText = SearchTextBox.Text.ToLower();
            var statusFilter = GetStatusFilter();
            var sortDescending = SortComboBox.SelectedIndex == 0;

            if (listViewReq == null || PageInfo == null || StatusComboBox == null || SortComboBox == null)
            {
                MessageBox.Show("Элементы интерфейса не инициализированы.");
                return;
            }

            var requestsQuery = KonfigKc.Requests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                if (int.TryParse(searchText, out int requestId))
                {
                    requestsQuery = requestsQuery.Where(x => x.RequestID == requestId);
                }
                else
                {
                    MessageBox.Show("Введите корректный номер заявки.");
                }
            }

            if (statusFilter != -1)
            {
                requestsQuery = requestsQuery.Where(x => x.StatusID == statusFilter);
            }

            if (sortDescending)
            {
                requestsQuery = requestsQuery.OrderBy(x => x.RequestDateStart);
            }
            else
            {
                requestsQuery = requestsQuery.OrderByDescending(x => x.RequestDateStart);
            }

            var requests = requestsQuery
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listViewReq.ItemsSource = requests;

            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)requestsQuery.Count() / PageSize)}";
        }

        private int GetStatusFilter()
        {
            if (StatusComboBox.SelectedItem == null)
            {
                return -1;
            }

            var selectedComboBoxItem = (ComboBoxItem)StatusComboBox.SelectedItem;

            if (selectedComboBoxItem.Content.ToString() == "Все")
            {
                return -1;
            }

            var selectedStatusName = selectedComboBoxItem.Content.ToString();

            var selectedStatus = KonfigKc.StatusRequests.FirstOrDefault(s => s.StatusName == selectedStatusName);

            return selectedStatus != null ? selectedStatus.StatusID : -1;
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayPage();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < (KonfigKc.Requests.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
                DisplayPage();
            }
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewReq.SelectedItem != null)
            {
                Request selectedReq = (Request)listViewReq.SelectedItem;
                EditPageJornul editPageJornul = new EditPageJornul(selectedReq);
                NavigationService.Navigate(editPageJornul);
            }
        }

        private void Page_IsVis(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DisplayPage();
            }
        }

        private void Create_Pdf_Click(object sender, RoutedEventArgs e)
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

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FormPage());
            LoadStatuses();
            DisplayPage();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPage = 1;
            DisplayPage();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPage = 1;
            DisplayPage();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPage = 1;
            DisplayPage();
        }

        private void ExportToWord_Click(object sender, RoutedEventArgs e)
        {
            var requests = listViewReq.Items.Cast<Request>().ToList();
            if (!requests.Any())
            {
                MessageBox.Show("Нет данных для экспорта.");
                return;
            }

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "ExportedRequests",
                DefaultExt = ".docx",
                Filter = "Word documents (.docx)|*.docx"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result != true) return;

            string filePath = saveFileDialog.FileName;

            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = new Body();
                    mainPart.Document.Append(body);

                    DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();

                    TableProperties tblProp = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                        )
                    );
                    table.AppendChild(tblProp);

                    // Table headers
                    DocumentFormat.OpenXml.Wordprocessing.TableRow headerRow = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                    AddTableCell(headerRow, "№");
                    AddTableCell(headerRow, "Подразделение");
                    AddTableCell(headerRow, "Должность");
                    AddTableCell(headerRow, "Дата выполнения");
                    AddTableCell(headerRow, "Описание");
                    AddTableCell(headerRow, "Статус");
                    AddTableCell(headerRow, "Кабинет");
                    AddTableCell(headerRow, "Номер");
                    AddTableCell(headerRow, "Дедлайн");
                    table.Append(headerRow);

                    // Table rows
                    foreach (var request in requests)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow row = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                        AddTableCell(row, request.RequestID.ToString());
                        AddTableCell(row, request.Client.Department?.DepartmentName ?? string.Empty);
                        AddTableCell(row, request.Client.Position?.PositionName ?? string.Empty);
                        AddTableCell(row, request.RequestDateFinish);
                        AddTableCell(row, request.Description);
                        AddTableCell(row, request.StatusRequest.StatusName);
                        AddTableCell(row, request.Client.Cabinet);
                        AddTableCell(row, request.Client.NumberPhone);
                        AddTableCell(row, request.RequestDeadline);
                        table.Append(row);
                    }

                    body.Append(table);
                    mainPart.Document.Save();
                }

                MessageBox.Show($"Данные успешно экспортированы в {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}");
            }
        }

        private void AddTableCell(DocumentFormat.OpenXml.Wordprocessing.TableRow row, string text)
        {
            DocumentFormat.OpenXml.Wordprocessing.TableCell cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(text))));
            row.Append(cell);
        }

        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            var requests = listViewReq.Items.Cast<Request>().ToList();
            if (!requests.Any())
            {
                MessageBox.Show("Нет данных для экспорта.");
                return;
            }

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "ExportedRequests",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result != true) return;

            string filePath = saveFileDialog.FileName;

            try
            {
                using (PdfDocument document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    page.Orientation = PdfSharp.PageOrientation.Landscape;

                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont font = new XFont("Times New Roman", 12);
                    XPen pen = new XPen(XColors.Black, 0.5);
                    XTextFormatter tf = new XTextFormatter(gfx);

                    double yPoint = 40;
                    double cellHeight = 20;
                    double[] columnWidths = { 30, 120, 100, 100, 150, 100, 50, 70, 100 };

                    // Table headers
                    string[] headers = { "№", "Подразделение", "Должность", "Дата выполнения", "Описание", "Статус", "Кабинет", "Номер", "Дедлайн" };
                    double xPoint = 10;

                    for (int i = 0; i < headers.Length; i++)
                    {
                        gfx.DrawRectangle(pen, xPoint, yPoint, columnWidths[i], cellHeight);
                        tf.DrawString(headers[i], font, XBrushes.Black, new XRect(xPoint + 5, yPoint + 5, columnWidths[i], cellHeight), XStringFormats.TopLeft);
                        xPoint += columnWidths[i];
                    }

                    yPoint += cellHeight;

                    // Table rows
                    foreach (var request in requests)
                    {
                        xPoint = 10;
                        string[] values = {
                    request.RequestID.ToString(),
                    request.Client?.Department?.DepartmentName ?? string.Empty,
                    request.Client?.Position?.PositionName ?? string.Empty,
                    request.RequestDateFinish ?? string.Empty,
                    request.Description ?? string.Empty,
                    request.StatusRequest?.StatusName ?? string.Empty,
                    request.Client?.Cabinet ?? string.Empty,
                    request.Client?.NumberPhone ?? string.Empty,
                    request.RequestDeadline ?? string.Empty
                };

                        for (int i = 0; i < values.Length; i++)
                        {
                            double cellYPoint = yPoint;
                            gfx.DrawRectangle(pen, xPoint, cellYPoint, columnWidths[i], cellHeight);
                            tf.DrawString(values[i], font, XBrushes.Black, new XRect(xPoint + 5, cellYPoint + 5, columnWidths[i], cellHeight), XStringFormats.TopLeft);
                            xPoint += columnWidths[i];
                        }

                        yPoint += cellHeight;
                    }

                    document.Save(filePath);
                }

                MessageBox.Show($"Данные успешно экспортированы в {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}");
            }
        }


    }
}
