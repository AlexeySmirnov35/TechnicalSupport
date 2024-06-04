using System;
using System.Collections.Generic;
using System.Diagnostics;
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
namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AddEditOperSystemWindow.xaml
    /// </summary>
    public partial class AddEditOperSystemWindow : Window
    {
        private readonly ApplicationContext _konfigKcDB;
        private readonly DataBaseClasses.OperatingSystem _operatingSystem;

        public AddEditOperSystemWindow(DataBaseClasses.OperatingSystem operatingSystem)
        {
            InitializeComponent();
            _konfigKcDB = new ApplicationContext();
            _operatingSystem = operatingSystem ?? new DataBaseClasses.OperatingSystem();
            DataContext = _operatingSystem;
            LoadFiles();
            if (operatingSystem != null)
            {
                tbName.Text = _operatingSystem.NameOperatingSystem;
                cbFile.SelectedItem = _konfigKcDB.FilesSoftwares.FirstOrDefault(os => os.FileID == _operatingSystem.FileID);
            }
        }

        private void LoadFiles()
        {
            cbFile.ItemsSource = _konfigKcDB.FilesSoftwares.ToList();
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

        private void Btn_Save_Soft_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tbName.Text))
                errors.AppendLine("Укажите название программы!");

            if (!Uri.TryCreate(tbWeb.Text, UriKind.Absolute, out var uriResult) || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                errors.AppendLine("Укажите корректную ссылку на документацию!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var prog = cbFile.SelectedItem as FilesSoftware;
            if (prog == null)
            {
                MessageBox.Show("Выберите файл программы!");
                return;
            }

            var isDuplicate = _konfigKcDB.OperatingSystems.Any(s => s.NameOperatingSystem == _operatingSystem.NameOperatingSystem && s.OperatingSystemsID != _operatingSystem.OperatingSystemsID);

            if (isDuplicate)
            {
                MessageBox.Show("Такая запись существует");
                return;
            }

            if (_operatingSystem.OperatingSystemsID == 0)
            {
                var newSoftware = new DataBaseClasses.OperatingSystem
                {
                    NameOperatingSystem = tbName.Text,
                    WebUrl = tbWeb.Text,
                    FileID = prog.FileID
                };

                _konfigKcDB.OperatingSystems.Add(newSoftware);
            }
            else
            {
                _operatingSystem.NameOperatingSystem = tbName.Text;
                _operatingSystem.WebUrl = tbWeb.Text;
                _operatingSystem.FileID = prog.FileID;
            }

            try
            {
                _konfigKcDB.SaveChanges();
                MessageBox.Show("Успешно сохранено");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            AddEditFileWindow addEditFileWindow = new AddEditFileWindow(null);
            addEditFileWindow.ShowDialog();
        }
    }
}
