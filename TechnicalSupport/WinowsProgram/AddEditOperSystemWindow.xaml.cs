using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AddEditOperSystemWindow.xaml
    /// </summary>
    public partial class AddEditOperSystemWindow : Window
    {
        private readonly ApplicationContext _konfigKcDB;
        private readonly DataBaseClasses.OperatingSystem _originalOperatingSystem;
        private readonly DataBaseClasses.OperatingSystem _editableOperatingSystem;

        public AddEditOperSystemWindow(DataBaseClasses.OperatingSystem operatingSystem, ApplicationContext konfigKcDB)
        {
            InitializeComponent();
            _konfigKcDB = konfigKcDB;
            _originalOperatingSystem = operatingSystem ?? new DataBaseClasses.OperatingSystem();
            _editableOperatingSystem = new DataBaseClasses.OperatingSystem
            {
                OperatingSystemsID = _originalOperatingSystem.OperatingSystemsID,
                NameOperatingSystem = _originalOperatingSystem.NameOperatingSystem,
                WebUrl = _originalOperatingSystem.WebUrl,
                FileID = _originalOperatingSystem.FileID
            };
            DataContext = _editableOperatingSystem;
            LoadFiles();
            if (operatingSystem != null)
            {
                tbName.Text = _editableOperatingSystem.NameOperatingSystem;
                tbWeb.Text = _editableOperatingSystem.WebUrl;
                cbFile.SelectedValue = _editableOperatingSystem.FileID;
            }
        }

        private void LoadFiles()
        {
            cbFile.ItemsSource = _konfigKcDB.FilesSoftwares.ToList();
            cbFile.SelectedValuePath = "FileID";
            cbFile.DisplayMemberPath = "FileName";
            cbFile.SelectedValue = _editableOperatingSystem.FileID;
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

        private async void Btn_Save_Soft_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = ValidateInputs();

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

            var isDuplicate = _konfigKcDB.OperatingSystems.Any(s => s.NameOperatingSystem == _editableOperatingSystem.NameOperatingSystem && s.OperatingSystemsID != _editableOperatingSystem.OperatingSystemsID);

            if (isDuplicate)
            {
                MessageBox.Show("Такая запись существует");
                return;
            }

            if (_editableOperatingSystem.OperatingSystemsID == 0)
            {
                var newOperatingSystem = new DataBaseClasses.OperatingSystem
                {
                    NameOperatingSystem = tbName.Text,
                    WebUrl = tbWeb.Text,
                    FileID = prog.FileID
                };

                _konfigKcDB.OperatingSystems.Add(newOperatingSystem);
            }
            else
            {
                _originalOperatingSystem.NameOperatingSystem = _editableOperatingSystem.NameOperatingSystem;
                _originalOperatingSystem.WebUrl = _editableOperatingSystem.WebUrl;
                _originalOperatingSystem.FileID = prog.FileID;
            }

            try
            {
                await _konfigKcDB.SaveChangesAsync();
                MessageBox.Show("Успешно сохранено");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private StringBuilder ValidateInputs()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tbName.Text))
                errors.AppendLine("Укажите название программы!");

            if (!Uri.TryCreate(tbWeb.Text, UriKind.Absolute, out var uriResult) || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                errors.AppendLine("Укажите корректную ссылку на документацию!");

            return errors;
        }

        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            var addEditFileWindow = new AddEditFileWindow(null, _konfigKcDB);
            addEditFileWindow.ShowDialog();
            LoadFiles();
        }
    }
}
