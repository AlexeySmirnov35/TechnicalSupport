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
using TechnicalSupport.WinowsProgram;
using TechnicalSupport.DataBaseClasses;
namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для RolePage.xaml
    /// </summary>
    public partial class RolePage : Page
    {
        ApplicationContext KonfigKc;
        private int currentPage = 1;
        private const int PageSize = 10;

        public RolePage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            LoadDepartments();
            DisplayPage();

        }
       
       

       

        private void LoadDepartments()
        {
            // Получаем все данные из базы данных
            listview.ItemsSource = KonfigKc.Positions.ToList();
        }

        private void DisplayPage()
        {
            // Получаем текущую страницу данных
            var departments = KonfigKc.Positions
                .OrderBy(d => d.PositionID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;

            // Обновляем текст с информацией о текущей странице
            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKc.Positions.Count() / PageSize)}";
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
            if (currentPage < (KonfigKc.Positions.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
                DisplayPage();
            }
        }
        private void DelRole_Click(object sender, RoutedEventArgs e)
        {
            var positionsToDelete = listview.SelectedItems.Cast<Position>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить эти {positionsToDelete.Count()} элемента!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var dbContext = KonfigKc;

                foreach (var position in positionsToDelete)
                {
                   /* if (!IsPositionUsedInOtherTables(position))
                    {
                        dbContext.Positions.Remove(position);
                    }
                    else
                    {
                        MessageBox.Show($"Должность {position.PositionName} используется в других таблицах и не может быть удалена.");
                    }*/
                }

                dbContext.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                listview.ItemsSource = dbContext.Positions.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении должности: {ex.Message}");
            }
        }

        private void AddEditRole_Click(object sender, RoutedEventArgs e)
        {
            AddEditRoleWindow addEditRoleWindow = new AddEditRoleWindow(null);
            addEditRoleWindow.ShowDialog();
            DisplayPage();
        }
       /* private bool IsPositionUsedInOtherTables(Position position)
        {
            var dbContext = KonfigKc;
         //  return dbContext.SoftwarePositions.Any(item => item.PositionID == position.PositionID)|| dbContext.Requests.Any(item => item.PositionID == position.PositionID);
        }*/

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var departmentsToDelete = (sender as Button).DataContext as Position;

            if (MessageBox.Show($"Вы действительно хотите удалить {departmentsToDelete.PositionName}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }
            // KonfigKcDB.Departments.Remove(del);
            try
            {

                if (!KonfigKc.Users.Any(item => item.PositionsID == departmentsToDelete.PositionID))
                {
                    KonfigKc.Positions.Remove(departmentsToDelete);
                    Console.WriteLine($"Удалено подразделение: {departmentsToDelete.PositionName}");
                    MessageBox.Show("Удаление прошло успешно");
                }
                else
                {
                    MessageBox.Show($"{departmentsToDelete.PositionName} используется в других таблицах и не может быть удален.");
                    Console.WriteLine($"{departmentsToDelete.PositionName} используется в других таблицах и не может быть удален.");
                }


                KonfigKc.SaveChanges();
                LoadDepartments();
                DisplayPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении подразделения: {ex.Message}");
                Console.WriteLine($"Ошибка при удалении подразделения: {ex}");
            }
        }

        private void EditRole_Click(object sender, RoutedEventArgs e)
        {
            AddEditRoleWindow addEditRoleWindow = new AddEditRoleWindow((sender as Button).DataContext as Position);
            addEditRoleWindow.ShowDialog();
            DisplayPage();
        }
    }
}

