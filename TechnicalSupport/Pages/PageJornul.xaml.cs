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
            LoadDepartments();
            DisplayPage();
        }

        private void LoadDepartments()
        {
            // Получаем все данные из базы данных
            listViewReq.ItemsSource = KonfigKc.Requests.ToList();
        }

        private void DisplayPage()
        {
            // Получаем текущую страницу данных
            var departments = KonfigKc.Requests
                .OrderBy(d => d.RequestID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
            .ToList();

            listViewReq.ItemsSource = departments;

            // Обновляем текст с информацией о текущей странице
            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKc.Requests.Count() / PageSize)}";
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
                LoadDepartments();
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
        }
    }
}
