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
using System.Diagnostics;
using System.IO;
namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для OfficeEquipPage.xaml
    /// </summary>
    public partial class OfficeEquipPage : Page
    {
        private ApplicationContext KonfigKcDB;
        private int currentPage = 1;
        private const int PageSize = 10;
        public OfficeEquipPage()
        {
            InitializeComponent();
            KonfigKcDB = new ApplicationContext();
            LoadDepartments();
            DisplayPage();

            Console.WriteLine("Страница DepartPage успешно загружена.");
        }
       

      

        private void LoadDepartments()
        {
            listview.ItemsSource = KonfigKcDB.OfficeEquipments.ToList();
        }

        private void DisplayPage()
        {
            var departments = KonfigKcDB.OfficeEquipments
                .OrderBy(d => d.OfficeEquipmentID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;

            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKcDB.OfficeEquipments.Count() / PageSize)}";
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
            if (currentPage < (KonfigKcDB.OfficeEquipments.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
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
            var departmentsToDelete = listview.SelectedItems.Cast<Department>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {departmentsToDelete.Count()} элемент(ов)?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                /*foreach (var department in departmentsToDelete)
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
                }*/

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

        private void Role_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RolePage());
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            AddEditOfficeEquipWindow addEditOfficeEquipWindow= new AddEditOfficeEquipWindow(null);
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
                var dbContext = KonfigKcDB;

                if (dbContext.PositionOfficeEquips.Any(item => item.OfficeEquipID == filesToDelete.OfficeEquipmentID) )
                {
                    MessageBox.Show($"Орг техника {filesToDelete.NameOfficeEquipment} используется в других таблицах и не может быть удален.");
                    return;
                }

                dbContext.OfficeEquipments.Remove(filesToDelete);

                dbContext.SaveChanges();
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
            var v=(sender as Button).DataContext as OfficeEquipment;
            AddEditOfficeEquipWindow addEditOfficeEquipWindow = new AddEditOfficeEquipWindow(v);
            addEditOfficeEquipWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
        }

        private void Btn_OpenFile(object sender, RoutedEventArgs e)
        {
            var sel = (sender as Button).DataContext as OfficeEquipment;
            
            if (sel != null)
            {
                try
                {
                    var dbContext = KonfigKcDB;
                    var file = dbContext.FilesSoftwares.FirstOrDefault(f => f.FileID == sel.FileID);
                    if (file != null)
                    {
                        string tempFilePath = System.IO.Path.GetTempFileName();
                        File.WriteAllBytes(tempFilePath, file.FileContent);
                        System.Diagnostics.Process.Start("rundll32.exe", $"shell32.dll,OpenAs_RunDLL {tempFilePath}");
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
