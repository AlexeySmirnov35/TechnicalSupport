using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public partial class UserSoft : Page
    {
        private readonly ApplicationContext _context;
        private int _currentPage = 1;
        private const int PageSize = 10;

        public UserSoft()
        {
            InitializeComponent();
            _context = new ApplicationContext();
            InitializeUIComponents();
        }

        private void InitializeUIComponents()
        {
            LoadDepartments();
            DisplayPage();
            InitializeSoftwareTypes();
        }

        private void InitializeSoftwareTypes()
        {
            var softwareTypes = _context.TypeSofwares
                .OrderBy(t => t.NameType)
                .ToList();

            var allType = new TypeSofware { TypeSofwareID = -1, NameType = "Все" };
            softwareTypes.Insert(0, allType);

            ComboBoxSoftwareType.ItemsSource = softwareTypes;
            ComboBoxSoftwareType.SelectedItem = allType;
        }

        private void UpdateSoftware()
        {
            var allSoftware = _context.Softwares.AsQueryable();

            if (!string.IsNullOrWhiteSpace(TboxSerch.Text))
            {
                allSoftware = allSoftware.Where(s => s.SoftwareName.ToLower().Contains(TboxSerch.Text.ToLower()));
            }

            if (ComboBoxSoftwareType.SelectedItem is TypeSofware selectedType && selectedType.TypeSofwareID != -1)
            {
                allSoftware = allSoftware.Where(s => s.TypeSofwareID == selectedType.TypeSofwareID);
            }

            listview.ItemsSource = allSoftware.OrderBy(s => s.SoftwareName).ToList();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (Uri.TryCreate(e.Uri.ToString(), UriKind.Absolute, out Uri uriResult))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = uriResult.ToString(),
                    UseShellExecute = true
                });

                e.Handled = true;
            }
            else
            {
                Debug.WriteLine("Некорректный URL");
            }
        }

        private void Btn_OpenFile(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Software selectedSoftware)
            {
                OpenFile(selectedSoftware.FileID);
            }
            else
            {
                MessageBox.Show("Файл не загружен");
            }
        }

        private void OpenFile(int? fileId)
        {
            try
            {
                var file = _context.FilesSoftwares.FirstOrDefault(f => f.FileID == fileId);
                if (file != null)
                {
                    string tempFilePath = System.IO.Path.GetTempFileName();
                    File.WriteAllBytes(tempFilePath, file.FileContent);
                    Process.Start("rundll32.exe", $"shell32.dll,OpenAs_RunDLL {tempFilePath}");
                }
                else
                {
                    MessageBox.Show("Файл не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия: {ex.Message}");
            }
        }

        private void LoadDepartments()
        {
            listview.ItemsSource = _context.Softwares.ToList();
        }

        private void DisplayPage()
        {
            var departments = _context.Softwares
                .OrderBy(d => d.SoftwareID)
                .Skip((_currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;
            PageInfo.Text = $"Страница {_currentPage} из {Math.Ceiling((double)_context.Softwares.Count() / PageSize)}";
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
            if (_currentPage < (_context.Softwares.Count() + PageSize - 1) / PageSize)
            {
                _currentPage++;
                DisplayPage();
            }
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview.SelectedItem is Software selectedSoftware)
            {
                var userPageSoft = new UserPageSoft(selectedSoftware);
                NavigationService.Navigate(userPageSoft);
            }
        }

        private void Tbox_Search(object sender, TextChangedEventArgs e)
        {
            UpdateSoftware();
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Software softwareToDelete)
            {
                ConfirmAndDeleteSoftware(softwareToDelete);
            }
            InitializeUIComponents();
        }

        private void ConfirmAndDeleteSoftware(Software software)
        {
            if (MessageBox.Show($"Вы действительное хотите удалите: {software.SoftwareName}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                if (_context.SoftwarePositions.Any(item => item.SoftwareID == software.SoftwareID))
                {
                    MessageBox.Show($" {software.SoftwareName} нелзя удалить.");
                    return;
                }

                _context.Softwares.Remove(software);
                _context.SaveChanges();
                MessageBox.Show("Удаленение прошло успешно");
                LoadDepartments();
                DisplayPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
            InitializeUIComponents();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Software softwareToEdit)
            {
                var addEditSoftwareWindow = new AddEditSoftwareWindow(softwareToEdit, _context);
                addEditSoftwareWindow.ShowDialog();
                InitializeUIComponents();
            }
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            var addEditSoftwareWindow = new AddEditSoftwareWindow(null, _context);
            addEditSoftwareWindow.ShowDialog();
            InitializeUIComponents();
        }

        private void ComboBoxSoftwareType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSoftware();
        }
    }
}
