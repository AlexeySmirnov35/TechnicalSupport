using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для AddEditOfficeEquipWindow.xaml
    /// </summary>
    public partial class AddEditOfficeEquipWindow : Window
    {
        private readonly ApplicationContext _konfigKcDB;
        private readonly OfficeEquipment _officeEquipment;

        public AddEditOfficeEquipWindow(OfficeEquipment officeEquipment = null)
        {
            InitializeComponent();
            _konfigKcDB = new ApplicationContext();
            _officeEquipment = officeEquipment ?? new OfficeEquipment();
            DataContext = _officeEquipment;
            LoadFiles();
            if (officeEquipment != null)
            {
                tbName.Text = _officeEquipment.NameOfficeEquipment;
                cbFile.SelectedItem = _konfigKcDB.FilesSoftwares.FirstOrDefault(os => os.FileID == _officeEquipment.FileID);
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

            if (IsDuplicateRecord())
            {
                MessageBox.Show("Такая запись существует");
                return;
            }

            if (_officeEquipment.OfficeEquipmentID == 0)
            {
                var newOfficeEquip = new OfficeEquipment
                {
                    NameOfficeEquipment = tbName.Text,
                    WebUrl = tbWeb.Text,
                    FileID = prog.FileID
                };

                _konfigKcDB.OfficeEquipments.Add(newOfficeEquip);
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

        private bool IsDuplicateRecord()
        {
            return _konfigKcDB.OfficeEquipments.Any(s => s.NameOfficeEquipment == _officeEquipment.NameOfficeEquipment && s.OfficeEquipmentID != _officeEquipment.OfficeEquipmentID);
        }

        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            AddEditFileWindow addEditFileWindow = new AddEditFileWindow(null);
            addEditFileWindow.ShowDialog();
        }
    }
}
