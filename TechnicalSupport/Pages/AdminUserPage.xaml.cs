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
using static MaterialDesignThemes.Wpf.Theme;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminUserPage.xaml
    /// </summary>
    public partial class AdminUserPage : Page
    {
        private ApplicationContext KonfigKcDB;
        private int currentPage = 1;
        private const int PageSize = 10;
        private User _user;
        public AdminUserPage(User user)
        {
            InitializeComponent();
            KonfigKcDB = new ApplicationContext();
            _user = user;
            LoadDepartments();
            DisplayPage();
        }
       

     

        private void LoadDepartments()
        {
            listView.ItemsSource = KonfigKcDB.Users.Where(x=>x.RoleID!=1).ToList();
        }

        private void DisplayPage()
        {
            var departments = KonfigKcDB.Users.Where(x => x.RoleID != 1)
                .OrderBy(d => d.UserID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listView.ItemsSource = departments;

            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKcDB.Users.Where(x => x.RoleID != 1).Count() / PageSize)}";
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
            if (currentPage < (KonfigKcDB.Users.Where(x => x.RoleID != 1).Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
                DisplayPage();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var d = (sender as System.Windows.Controls.Button).DataContext as User;
            AdminUserWindow addEditDepartWindow = new AdminUserWindow(d, KonfigKcDB, _user);

            addEditDepartWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // var departmentsToDelete = listview.SelectedItems.Cast<Department>().ToList();
            var departmentsToDelete = (sender as System.Windows.Controls.Button).DataContext as User;

            if (MessageBox.Show($"Вы действительно хотите удалить {departmentsToDelete.Firstname} {departmentsToDelete.Surname}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }
            // KonfigKcDB.Departments.Remove(del);
            try
            {

                if (!KonfigKcDB.Requests.Any(item => item.UserID == departmentsToDelete.UserID))
                {
                    KonfigKcDB.Users.Remove(departmentsToDelete);
                    Console.WriteLine($"Удален сотрудник {departmentsToDelete.Firstname} {departmentsToDelete.Surname}");
                    MessageBox.Show("Удаление прошло успешно");
                }
                else
                {
                    MessageBox.Show($"{departmentsToDelete.Firstname} {departmentsToDelete.Surname} выполняет заявки.");
                    Console.WriteLine($"{departmentsToDelete.Firstname}   {departmentsToDelete.Surname} используется в других таблицах и не может быть удален.");
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

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            AdminUserWindow addEditDepartWindow = new AdminUserWindow(null, KonfigKcDB, _user);
            addEditDepartWindow.ShowDialog();

            DisplayPage();
        }
    }
}
