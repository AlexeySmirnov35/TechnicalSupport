using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Interaction logic for MasterAreaPage.xaml
    /// </summary>
    public partial class MasterAreaPage : Page
    {
        private readonly ApplicationContext _context;
        private readonly User _user;
        private int _currentPage = 1;
        private const int PageSize = 10;

        public MasterAreaPage(User user)
        {
            InitializeComponent();
            _context = new ApplicationContext();
            _user = user;
            LoadRequests();
        }

        private void LoadRequests()
        {
            var requests = _context.Requests
                .Where(x => x.UserID == _user.UserID && x.StatusID < 4)
                .ToList();

            listViewReq.ItemsSource = requests;
            UpdatePageInfo();
        }

        private void UpdatePageInfo()
        {
            var totalRequests = _context.Requests
                .Where(x => x.UserID == _user.UserID && x.StatusID < 4)
                .Count();

            PageInfo.Text = $"Страница {_currentPage} из {Math.Ceiling((double)totalRequests / PageSize)}";
        }

        private void DisplayPage()
        {
            var requests = _context.Requests
                .Where(x => x.UserID == _user.UserID && x.StatusID < 4)
                .OrderBy(d => d.RequestID)
                .Skip((_currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listViewReq.ItemsSource = requests;
            UpdatePageInfo();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                DisplayPage();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            var totalRequests = _context.Requests
                .Where(x => x.UserID == _user.UserID && x.StatusID < 4)
                .Count();

            if (_currentPage < (totalRequests + PageSize - 1) / PageSize)
            {
                _currentPage++;
                DisplayPage();
            }
        }

        private void listViewReq_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewReq.SelectedItem is Request selectedRequest)
            {
                var editPage = new EditPageJornul(selectedRequest);
                NavigationService.Navigate(editPage);
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                LoadRequests();
                DisplayPage();
            }
        }

        private void Create_Pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsEnabled = false;
                var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "Invoice");
                }
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private void EditDoneRole_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Request request)
            {
                request.StatusID = 3;
                request.RequestDateFinish = DateTime.Now.ToString();
                _context.SaveChanges();
                LoadRequests();
                DisplayPage();
            }
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
