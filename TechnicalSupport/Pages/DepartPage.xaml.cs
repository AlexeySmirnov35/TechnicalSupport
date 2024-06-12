using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TechnicalSupport;
using TechnicalSupport.WinowsProgram;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.Pages
{
    public partial class DepartPage : Page
    {
        private readonly ApplicationContext _konfigKcDB;
        private int _currentPage = 1;
        private const int PageSize = 10;

        public DepartPage()
        {
            InitializeComponent();
            _konfigKcDB = new ApplicationContext();
            LoadDepartments();
            DisplayPage();
        }

        private void LoadDepartments()
        {
            listview.ItemsSource = _konfigKcDB.Departments.ToList();
        }

        private void DisplayPage()
        {
            var departments = _konfigKcDB.Departments
                .OrderBy(d => d.DepartmentID)
                .Skip((_currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;

            PageInfo.Text = $"Страница {_currentPage} из {Math.Ceiling((double)_konfigKcDB.Departments.Count() / PageSize)}";
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
            if (_currentPage < (_konfigKcDB.Departments.Count() + PageSize - 1) / PageSize)
            {
                _currentPage++;
                DisplayPage();
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DelDepar_Click(object sender, RoutedEventArgs e)
        {
            var departmentsToDelete = listview.SelectedItems.Cast<Department>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {departmentsToDelete.Count()} элемент(ов)?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                foreach (var department in departmentsToDelete)
                {
                    if (!_konfigKcDB.Users.Any(item => item.DepartmentID == department.DepartmentID))
                    {
                        _konfigKcDB.Departments.Remove(department);
                        Console.WriteLine($"Удалено подразделение: {department.DepartmentName}");
                    }
                    else
                    {
                        MessageBox.Show($"{department.DepartmentName} используется в других таблицах и не может быть удален.");
                        Console.WriteLine($"{department.DepartmentName} используется в других таблицах и не может быть удален.");
                    }
                }

                _konfigKcDB.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
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
            var addEditDepartWindow = new AddEditDepartWindow(null, _konfigKcDB);
            addEditDepartWindow.ShowDialog();

            DisplayPage();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var departmentsToDelete = (sender as Button).DataContext as Department;

            if (MessageBox.Show($"Вы действительно хотите удалить {departmentsToDelete.DepartmentName}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                if (!_konfigKcDB.Clients.Any(item => item.DepartamentID == departmentsToDelete.DepartmentID))
                {
                    _konfigKcDB.Departments.Remove(departmentsToDelete);
                    Console.WriteLine($"Удалено подразделение: {departmentsToDelete.DepartmentName}");
                    MessageBox.Show("Удаление прошло успешно");
                }
                else
                {
                    MessageBox.Show($"{departmentsToDelete.DepartmentName} используется в других таблицах и не может быть удален.");
                    Console.WriteLine($"{departmentsToDelete.DepartmentName} используется в других таблицах и не может быть удален.");
                }

                _konfigKcDB.SaveChanges();
                LoadDepartments();
                DisplayPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении подразделения: {ex.Message}");
                Console.WriteLine($"Ошибка при удалении подразделения: {ex}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Department department)
            {
                var addCommitWindow = new AddEditDepartWindow(department, _konfigKcDB);
                addCommitWindow.ShowDialog();

                LoadDepartments();
                DisplayPage();
            }
        }
    }
}
