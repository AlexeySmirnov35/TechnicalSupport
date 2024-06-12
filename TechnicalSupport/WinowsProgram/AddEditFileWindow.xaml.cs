using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AddEditFileWindow.xaml
    /// </summary>
    public partial class AddEditFileWindow : Window
    {
        private byte[] _fileContent = null;
        private readonly ApplicationContext _context;
        private readonly FilesSoftware _originalFile;
        private readonly FilesSoftware _editableFile;

        public AddEditFileWindow(FilesSoftware filesSoftware, ApplicationContext context)
        {
            InitializeComponent();
            _context = context;
            _originalFile = filesSoftware ?? new FilesSoftware();
            _editableFile = new FilesSoftware
            {
                FileID = _originalFile.FileID,
                FileContent = _originalFile.FileContent,
                FileName = _originalFile.FileName,
            };
            DataContext = _editableFile;
        }

        private void Btn_AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileOpen = new OpenFileDialog
            {
                Title = "Выберите файлы",
                Multiselect = true,
                Filter = "Текстовые файлы|*.txt;*.docx;*.pdf|Все файлы|*.*"
            };

            if (fileOpen.ShowDialog() == true)
            {
                foreach (string fileName in fileOpen.FileNames)
                {
                    _fileContent = File.ReadAllBytes(fileName);
                    tbContent.Text += fileOpen.SafeFileName + "\n";
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
                var isDuplicate = _context.FilesSoftwares.Any(f => f.FileName == fileName && f.FileID != _editableFile.FileID);

                if (isDuplicate)
                {
                    errors.AppendLine("Такой файл уже существует.");
                }

                if (errors.Length == 0)
                {
                    if (_editableFile.FileID != 0)
                    {
                        var fileToUpdate = _context.FilesSoftwares.First(f => f.FileID == _editableFile.FileID);
                        fileToUpdate.FileName = fileName;
                        fileToUpdate.FileContent = _fileContent ?? fileToUpdate.FileContent;
                    }
                    else
                    {
                        FilesSoftware newFile = new FilesSoftware { FileName = fileName, FileContent = _fileContent };
                        _context.FilesSoftwares.Add(newFile);
                    }

                    _context.SaveChanges();
                    this.DialogResult = true;
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
