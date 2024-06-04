using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;
namespace TechnicalSupport.Pages
{
    public partial class FilePage : Page
    {
        private byte[] fileContent = null;
        private FilesSoftware selectedFile = null;
        ApplicationContext KonfigKc;
        private int currentPage = 1;
        private const int PageSize = 10;

        public FilePage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            LoadDepartments();
            DisplayPage();
        }
        private void LoadDepartments()
        {
            // Получаем все данные из базы данных
            listview.ItemsSource = KonfigKc.FilesSoftwares.ToList();
        }
        private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (listview.SelectedItem != null)
            {
                selectedFile = (FilesSoftware)listview.SelectedItem;
                //tbContent.Text = selectedFile.FileName;
            }
        }

        private void DisplayPage()
        {
            // Получаем текущую страницу данных
            var departments = KonfigKc.FilesSoftwares
                .OrderBy(d => d.FileID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            listview.ItemsSource = departments;

            // Обновляем текст с информацией о текущей странице
            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKc.FilesSoftwares.Count() / PageSize)}";
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
            if (currentPage < (KonfigKc.FilesSoftwares.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
                DisplayPage();
            }
        }
        private void Btn_GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Btn_DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private void Btn_OpenFile(object sender, RoutedEventArgs e)
        {
            var sel = (sender as Button).DataContext as FilesSoftware;
           
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

        private void AddEditFile_Click(object sender, RoutedEventArgs e)
        {
            AddEditFileWindow addEditFileWindow = new AddEditFileWindow(null);
            addEditFileWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var filesToDelete = (sender as Button).DataContext as FilesSoftware;

            if (MessageBox.Show($"Вы действительно хотите удалить этот файл {filesToDelete.FileName}!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var dbContext = KonfigKc;

                // Проверяем, используется ли файл в других таблицах
                if (dbContext.Softwares.Any(item => item.FileID == filesToDelete.FileID) || dbContext.OperatingSystems.Any(item => item.FileID == filesToDelete.FileID) || dbContext.OfficeEquipments.Any(item => item.FileID == filesToDelete.FileID))
                {
                    MessageBox.Show($"Файл {filesToDelete.FileName} используется в других таблицах и не может быть удален.");
                    return;
                }

                

                // Удаляем объект
                dbContext.FilesSoftwares.Remove(filesToDelete);

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
            var v=(sender as Button).DataContext as FilesSoftware;
            AddEditFileWindow addEditFileWindow = new AddEditFileWindow(v);
            addEditFileWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();

        }
    }
}

