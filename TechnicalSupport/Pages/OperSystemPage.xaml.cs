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
using System.IO;
using System.Diagnostics;
namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для OperSystemPage.xaml
    /// </summary>
    public partial class OperSystemPage : Page
    {
        private ApplicationContext KonfigKcDB;
        private int currentPage = 1;
        private const int PageSize = 10;
        public OperSystemPage()
        {
            InitializeComponent(); 
            KonfigKcDB = new ApplicationContext();
            LoadDepartments();
            DisplayPage();

            Console.WriteLine("Страница DepartPage успешно загружена.");
        }


     

     

        private void LoadDepartments()
        {
            listview.ItemsSource = KonfigKcDB.OperatingSystems.ToList();
        }

        private void DisplayPage()
        {
            var departments = KonfigKcDB.OperatingSystems
                .OrderBy(d => d.OperatingSystemsID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;

            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKcDB.OperatingSystems.Count() / PageSize)}";
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
            if (currentPage < (KonfigKcDB.OperatingSystems.Count() + PageSize - 1) / PageSize)
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
            
        }

        private void Role_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RolePage());
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
          AddEditOperSystemWindow addEditOperSystemWindow=new AddEditOperSystemWindow(null, KonfigKcDB);
            addEditOperSystemWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var filesToDelete = (sender as Button).DataContext as DataBaseClasses.OperatingSystem;

            if (MessageBox.Show($"Вы действительно хотите удалить этот файл {filesToDelete.NameOperatingSystem}!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var dbContext = KonfigKcDB;

                if (dbContext.Positions.Any(item => item.OperatingSystemsID == filesToDelete.OperatingSystemsID) )
                {
                    MessageBox.Show($"ОС {filesToDelete.NameOperatingSystem} используется в других таблицах и не может быть удален.");
                    return;
                }

                dbContext.OperatingSystems.Remove(filesToDelete);

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
            var v=(sender as Button).DataContext as DataBaseClasses.OperatingSystem;
            AddEditOperSystemWindow addEditOperSystemWindow = new AddEditOperSystemWindow(v, KonfigKcDB);
            addEditOperSystemWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
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
            var sel = (sender as Button).DataContext as DataBaseClasses.OperatingSystem;

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
