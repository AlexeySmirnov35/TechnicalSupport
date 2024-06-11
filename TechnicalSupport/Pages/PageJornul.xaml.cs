using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

            ComboBoxItem allItem = new ComboBoxItem();
            allItem.Content = "Все";
            StatusComboBox.Items.Add(allItem);

            foreach (var status in statuses)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = status.StatusName; 
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
            NavigationService.Navigate(new FormPage()); LoadStatuses();
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
    }
}
