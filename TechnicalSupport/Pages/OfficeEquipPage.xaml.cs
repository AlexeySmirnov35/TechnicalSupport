using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для OfficeEquipPage.xaml
    /// </summary>
    public partial class OfficeEquipPage : Page
    {
        private readonly ApplicationContext _konfigKcDB;
        private int _currentPage = 1;
        private const int PageSize = 10;

        public OfficeEquipPage()
        {
            InitializeComponent();
            _konfigKcDB = new ApplicationContext();
            LoadDepartments();
            DisplayPage();

            Console.WriteLine("Страница OfficeEquipPage успешно загружена.");
        }

        private void LoadDepartments()
        {
            listview.ItemsSource = _konfigKcDB.OfficeEquipments.ToList();
        }

        private void DisplayPage()
        {
            var departments = _konfigKcDB.OfficeEquipments
                .OrderBy(d => d.OfficeEquipmentID)
                .Skip((_currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;

            PageInfo.Text = $"Страница {_currentPage} из {Math.Ceiling((double)_konfigKcDB.OfficeEquipments.Count() / PageSize)}";
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
            if (_currentPage < (_konfigKcDB.OfficeEquipments.Count() + PageSize - 1) / PageSize)
            {
                _currentPage++;
                DisplayPage();
            }
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

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DelDepar_Click(object sender, RoutedEventArgs e)
        {
            var departmentsToDelete = listview.SelectedItems.Cast<OfficeEquipment>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {departmentsToDelete.Count()} элемент(ов)?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                foreach (var department in departmentsToDelete)
                {
                    if (!_konfigKcDB.PositionOfficeEquips.Any(item => item.OfficeEquipID == department.OfficeEquipmentID))
                    {
                        _konfigKcDB.OfficeEquipments.Remove(department);
                        Console.WriteLine($"Удалено подразделение: {department.NameOfficeEquipment}");
                    }
                    else
                    {
                        MessageBox.Show($"{department.NameOfficeEquipment} используется в других таблицах и не может быть удален.");
                        Console.WriteLine($"{department.NameOfficeEquipment} используется в других таблицах и не может быть удален.");
                    }
                }

                _konfigKcDB.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                LoadDepartments();
                DisplayPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении подразделения: {ex.Message}");
                Console.WriteLine($"Ошибка при удалении подразделения: {ex}");
            }
        }

        private void Role_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RolePage());
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            AddEditOfficeEquipWindow addEditOfficeEquipWindow = new AddEditOfficeEquipWindow(null, _konfigKcDB);
            addEditOfficeEquipWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var filesToDelete = (sender as Button).DataContext as OfficeEquipment;

            if (MessageBox.Show($"Вы действительно хотите удалить этот файл {filesToDelete.NameOfficeEquipment}!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                if (_konfigKcDB.PositionOfficeEquips.Any(item => item.OfficeEquipID == filesToDelete.OfficeEquipmentID))
                {
                    MessageBox.Show($"Орг техника {filesToDelete.NameOfficeEquipment} используется в других таблицах и не может быть удален.");
                    return;
                }

                _konfigKcDB.OfficeEquipments.Remove(filesToDelete);

                _konfigKcDB.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                LoadDepartments();
                DisplayPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is OfficeEquipment officeEquipment)
            {
                var addEditOfficeEquipWindow = new AddEditOfficeEquipWindow(officeEquipment, _konfigKcDB);
                addEditOfficeEquipWindow.ShowDialog();
                LoadDepartments();
                DisplayPage();
            }
        }

        private void Btn_OpenFile(object sender, RoutedEventArgs e)
        {
            var sel = (sender as Button).DataContext as OfficeEquipment;

            if (sel != null)
            {
                try
                {
                    var file = _konfigKcDB.FilesSoftwares.FirstOrDefault(f => f.FileID == sel.FileID);
                    if (file != null)
                    {
                        string tempFilePath = System.IO.Path.GetTempFileName();
                        File.WriteAllBytes(tempFilePath, file.FileContent);
                        Process.Start("rundll32.exe", $"shell32.dll,OpenAs_RunDLL {tempFilePath}");
                    }
                    else
                    {
                        MessageBox.Show("Файл не найден для выбранной программы.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Файл не был загружен");
            }
        }
    }
}
