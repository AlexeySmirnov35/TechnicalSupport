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
    /// <summary>
    /// Логика взаимодействия для UserSoft.xaml
    /// </summary>
    public partial class UserSoft : Page
    {
        ApplicationContext KonfigKc;
        private int currentPage = 1;
        private const int PageSize = 10;

        public UserSoft()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
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
            var sel = (sender as Button).DataContext as Software;

            if (sel != null)
            {
                try
                {
                    var dbContext = KonfigKc;
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

        private void LoadDepartments()
        {
            // Получаем все данные из базы данных
            listview.ItemsSource = KonfigKc.Softwares.ToList();
        }

        private void DisplayPage()
        {
            // Получаем текущую страницу данных
            var departments = KonfigKc.Softwares
                .OrderBy(d => d.SoftwareID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;

            // Обновляем текст с информацией о текущей странице
            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKc.Softwares.Count() / PageSize)}";
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
            if (currentPage < (KonfigKc.Softwares.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
                DisplayPage();
            }
        }
        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview.SelectedItem != null)
            {
                Software selectedSoftware = (Software)listview.SelectedItem;
                UserPageSoft userPageSoft = new UserPageSoft(selectedSoftware);
                NavigationService.Navigate(userPageSoft);
                
            }
        }

        private void Tbox_Search(object sender, TextChangedEventArgs e)
        {
            UpdateSoftware();
        }
        private void UpdateSoftware()
        {
            var allSoftware = KonfigKc.Softwares.ToList();
            allSoftware = allSoftware.Where(s => s.SoftwareName.ToLower().Contains(tbox_Search.Text.ToLower())).ToList();
            listview.ItemsSource = allSoftware.OrderBy(s => s.SoftwareName).ToList();
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var filesToDelete = (sender as Button).DataContext as Software;

            if (MessageBox.Show($"Вы действительно хотите удалить этот файл {filesToDelete.SoftwareName}!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var dbContext = KonfigKc;

                // Проверяем, используется ли файл в других таблицах
                if (dbContext.SoftwarePositions.Any(item => item.SoftwareID == filesToDelete.SoftwareID))
                {
                    MessageBox.Show($"Программа {filesToDelete.SoftwareName} используется в других таблицах и не может быть удален.");
                    return;
                }



                // Удаляем объект
                dbContext.Softwares.Remove(filesToDelete);

                // Сохраняем изменения
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
            var d = (sender as Button).DataContext as Software;
            AddEditSoftwareWindow addEditDepartWindow = new AddEditSoftwareWindow(d);

            addEditDepartWindow.ShowDialog();
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            AddEditSoftwareWindow addEditSoftwareWindow = new AddEditSoftwareWindow(null);
            addEditSoftwareWindow.ShowDialog();
        }
    }
}
