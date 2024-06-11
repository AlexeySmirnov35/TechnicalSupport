using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using TechnicalSupport;
using TechnicalSupport.WinowsProgram;
using TechnicalSupport.DataBaseClasses;
using System.Data.Entity;

namespace TechnicalSupport.Pages
{
    public partial class DepartPage : Page
    {
        private ApplicationContext KonfigKcDB;
        private int currentPage = 1;
        private const int PageSize = 10;

        public DepartPage()
        {
            InitializeComponent();
            KonfigKcDB = new ApplicationContext();
            LoadDepartments();
            DisplayPage();
        }

        private void LoadDepartments()
        {
            listview.ItemsSource = KonfigKcDB.Departments.ToList();
        }

        private void DisplayPage()
        {
            var departments = KonfigKcDB.Departments
                .OrderBy(d => d.DepartmentID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            
            listview.ItemsSource = departments;

            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKcDB.Departments.Count() / PageSize)}";
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
            if (currentPage < (KonfigKcDB.Departments.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
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
                    /*if (!KonfigKcDB.Requests.Any(item => item.DepartmentID == department.DepartmentID))
                    {
                        KonfigKcDB.Departments.Remove(department);
                        Console.WriteLine($"Удалено подразделение: {department.DepartmentName}");
                    }
                    else
                    {
                        MessageBox.Show($"{department.DepartmentName} используется в других таблицах и не может быть удален.");
                        Console.WriteLine($"{department.DepartmentName} используется в других таблицах и не может быть удален.");
                    }*/
                }

                KonfigKcDB.SaveChanges();
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
            AddEditDepartWindow addEditDepartWindow = new AddEditDepartWindow(null);
            addEditDepartWindow.ShowDialog();
            
            DisplayPage();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
           // var departmentsToDelete = listview.SelectedItems.Cast<Department>().ToList();
            var departmentsToDelete = (sender as Button).DataContext as Department;
           
            if (MessageBox.Show($"Вы действительно хотите удалить {departmentsToDelete.DepartmentName}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }
           // KonfigKcDB.Departments.Remove(del);
            try
            {
                
                    if (!KonfigKcDB.Clients.Any(item => item.DepartamentID == departmentsToDelete.DepartmentID))
                    {
                        KonfigKcDB.Departments.Remove(departmentsToDelete);
                        Console.WriteLine($"Удалено подразделение: {departmentsToDelete.DepartmentName}");
                        MessageBox.Show("Удаление прошло успешно");
                    }
                    else
                    {
                        MessageBox.Show($"{departmentsToDelete.DepartmentName} используется в других таблицах и не может быть удален.");
                        Console.WriteLine($"{departmentsToDelete.DepartmentName} используется в других таблицах и не может быть удален.");
                    }
                

                KonfigKcDB.SaveChanges();
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
                AddEditDepartWindow addCommitWindow = new AddEditDepartWindow(department);
                addCommitWindow.ShowDialog();           
                
                LoadDepartments();
                DisplayPage();
            }
          
        }
    }
}
