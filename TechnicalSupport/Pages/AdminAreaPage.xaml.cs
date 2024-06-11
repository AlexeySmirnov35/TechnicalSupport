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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminAreaPage.xaml
    /// </summary>
    public partial class AdminAreaPage : Page
    {
        private ApplicationContext KonfigKc;
        private User _user = new User();
        private int currentPage = 1;
        private const int PageSize = 10;
        public AdminAreaPage(User user)
        {
             InitializeComponent();
            KonfigKc = new ApplicationContext();
            _user = user;
            LoadDepartments();
            DisplayPage();
        }
        

        private void LoadDepartments()
        {
            listViewReq.ItemsSource = KonfigKc.Requests.Where(x => x.StatusID == 4).ToList();

        }

        private void DisplayPage()
        {
            var departments = KonfigKc.Requests.Where(x => x.StatusID == 4)
                .OrderBy(d => d.RequestID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
            .ToList();

            listViewReq.ItemsSource = departments;

            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKc.Requests.Where(x => x.StatusID == 4).Count() / PageSize)}";
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
            if (currentPage < (KonfigKc.Requests.Where(x => x.StatusID == 4).Count() + PageSize - 1) / PageSize)
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
        private void EditNoDoneRole_Click(object sender, RoutedEventArgs e)
        {
            var r = (sender as Button).DataContext as Request;
            AddCommitWindow addCommitWindow = new AddCommitWindow(r,_user);
            addCommitWindow.ShowDialog();
            KonfigKc.SaveChanges();
            LoadDepartments();
            DisplayPage();
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

        private void EditDoneRole_Click(object sender, RoutedEventArgs e)
        {

            var r = (sender as Button).DataContext as Request;
            r.StatusID = 2;
            KonfigKc.SaveChanges();
            LoadDepartments();
            DisplayPage();

        }
    }
}
