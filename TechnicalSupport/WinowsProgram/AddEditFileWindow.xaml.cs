using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;
using TechnicalSupport.DataBaseClasses;
namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AddEditFileWindow.xaml
    /// </summary>
    public partial class AddEditFileWindow : Window
    {
        private byte[] fileContent = null;
        private FilesSoftware selectedFile = null;
        ApplicationContext KonfigKc;
        private FilesSoftware filesSoftware1 = new FilesSoftware();
        public AddEditFileWindow(FilesSoftware filesSoftware)
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            if(filesSoftware != null )
            {
                filesSoftware1 = filesSoftware;
            }
            DataContext = filesSoftware1;
           // listview.ItemsSource = KonfigKc.FilesSoftwares.ToList();
            //listview.SelectionChanged += Listview_SelectionChanged;
        }


        private void Btn_AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog();

            fileOpen.Title = "Выберите файлы";
            fileOpen.Multiselect = true;
            fileOpen.Filter = "Текстовые файлы|*.txt;*.docx;*.pdf|Все файлы|*.*";

            if (fileOpen.ShowDialog() == true)
            {
                foreach (string fileName in fileOpen.FileNames)
                {
                     fileContent = File.ReadAllBytes(fileName);
                    tbContent.Text += fileOpen.SafeFileName + "\n";
                    // Дополнительно можете сохранять или обрабатывать содержимое файла
                }
            }
        }

        private void Btn_SaveFile_Click(object sender, RoutedEventArgs e)
        {
            string fileName = tbContent.Text.Trim();
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(fileName))
            {
                errors.AppendLine("Выберите файл.");
            }
            else
            {
                var dbContext = KonfigKc;

                var isDuplicate = dbContext.FilesSoftwares.Any(f => f.FileName == fileName && f.FileID != filesSoftware1.FileID);

                if (isDuplicate)
                {
                    errors.AppendLine("Такой файл уже существует.");
                }

                if (errors.Length == 0)
                {
                    if (filesSoftware1.FileID != 0)
                    {
                        var fileToUpdate = dbContext.FilesSoftwares.First(f => f.FileID == filesSoftware1.FileID);
                        fileToUpdate.FileName = fileName;
                        fileToUpdate.FileContent = fileContent ?? fileToUpdate.FileContent;
                    }
                    else
                    {
                        FilesSoftware newFile = new FilesSoftware { FileName = fileName, FileContent = fileContent };
                        dbContext.FilesSoftwares.Add(newFile);
                    }

                    dbContext.SaveChanges();
                    this.DialogResult = true;

                    // Закройте окно после сохранения
                    this.Close();
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
            }
        }
    }
}
