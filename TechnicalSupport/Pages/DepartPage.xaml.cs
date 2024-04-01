using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TechnicalSupport;

namespace TechnicalSupport.Pages
{
    public partial class DepartPage : Page
    {
        ApplicationContext KonfigKcDB;

        public DepartPage()
        {
            InitializeComponent();
            KonfigKcDB = new ApplicationContext();
            listview.ItemsSource = KonfigKcDB.Departments.ToList();

            Console.WriteLine("Страница DepartPage успешно загружена.");
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            string newDeparTitle = tbDep.Text;
            if (string.IsNullOrEmpty(newDeparTitle))
            {
                MessageBox.Show("Введите подразделение");
                return;
            }

            try
            {
                Department newDep = new Department { DepartmentName = newDeparTitle };
                KonfigKcDB.Departments.Add(newDep);
                KonfigKcDB.SaveChanges();

                Console.WriteLine($"Добавлено новое подразделение: {newDep.DepartmentName}");
                listview.ItemsSource = KonfigKcDB.Departments.ToList();
                tbDep.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении подразделения: {ex.Message}");
                Console.WriteLine($"Ошибка при добавлении подразделения: {ex}");
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
                    if (!KonfigKcDB.Requests.Any(item => item.DepartmentID == department.DepartmentID))
                    {
                        KonfigKcDB.Departments.Remove(department);
                        Console.WriteLine($"Удалено подразделение: {department.DepartmentName}");
                    }
                    else
                    {
                        MessageBox.Show($"{department.DepartmentName} используется в других таблицах и не может быть удален.");
                        Console.WriteLine($"{department.DepartmentName} используется в других таблицах и не может быть удален.");
                    }
                }

                KonfigKcDB.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                listview.ItemsSource = KonfigKcDB.Departments.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении подразделения: {ex.Message}");
                Console.WriteLine($"Ошибка при удалении подразделения: {ex}");
            }
        }
    }
}
