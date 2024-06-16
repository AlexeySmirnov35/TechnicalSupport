using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        private SoftwarePosition _software = new SoftwarePosition();
        private ApplicationContext KonfigKc;
        private int currentPage = 1;
        private const int PageSize = 10;

        public InfoPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            LoadDepartments();
            DisplayPage();
        }

        private void LoadDepartments()
        {
            softwareListView.ItemsSource = KonfigKc.SoftwarePositions.ToList();
        }

        private void DisplayPage()
        {
            var departments = KonfigKc.SoftwarePositions
                .OrderBy(d => d.SoftwareProgPositionID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            softwareListView.ItemsSource = departments;
            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKc.SoftwarePositions.Count() / PageSize)}";
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
            if (currentPage < (KonfigKc.SoftwarePositions.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
                DisplayPage();
            }
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (softwareListView.SelectedItem != null)
            {
                SoftwarePosition selectedSoftware = (SoftwarePosition)softwareListView.SelectedItem;
                EditPageInformation editSoftwarePage = new EditPageInformation(selectedSoftware);
                NavigationService.Navigate(editSoftwarePage);
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSoftwareList = (sender as Button).DataContext as SoftwarePosition;

            if (MessageBox.Show($"Вы действительно хотите удалить элемент?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    KonfigKc.SoftwarePositions.Remove(selectedSoftwareList);
                    KonfigKc.SaveChanges();
                    MessageBox.Show("Удаление прошло успешно");
                    LoadDepartments();
                    DisplayPage();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении элементов: {ex.Message}");
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSoftwarePosition = (sender as Button).DataContext as SoftwarePosition;
            EditWindowSoftPosit editWindowSoftPosit = new EditWindowSoftPosit(selectedSoftwarePosition, KonfigKc);
            editWindowSoftPosit.ShowDialog();
            LoadDepartments();
            DisplayPage(); // Ensure the list is updated after edit
        }

        private void Btn_AddFile(object sender, RoutedEventArgs e)
        {
            AddInfoSotrPOWindow addInfoSotrPOWindow = new AddInfoSotrPOWindow();
            addInfoSotrPOWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
        }
    }
}
