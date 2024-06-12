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
    /// Логика взаимодействия для AddEditOfficeEquipWindow.xaml
    /// </summary>
    public partial class AddEditOfficeEquipWindow : Window
    {
        private readonly ApplicationContext _konfigKcDB;
        private readonly OfficeEquipment _originalOfficeEquipment;
        private readonly OfficeEquipment _editableOfficeEquipment;

        public AddEditOfficeEquipWindow(OfficeEquipment officeEquipment, ApplicationContext konfigKcDB)
        {
            InitializeComponent();
            _konfigKcDB = konfigKcDB;
            _originalOfficeEquipment = officeEquipment ?? new OfficeEquipment();
            _editableOfficeEquipment = new OfficeEquipment
            {
                OfficeEquipmentID = _originalOfficeEquipment.OfficeEquipmentID,
                NameOfficeEquipment = _originalOfficeEquipment.NameOfficeEquipment,
                WebUrl = _originalOfficeEquipment.WebUrl,
                FileID = _originalOfficeEquipment.FileID
            };
            DataContext = _editableOfficeEquipment;
            LoadFiles();

            if (officeEquipment != null)
            {
                tbName.Text = _editableOfficeEquipment.NameOfficeEquipment;
                tbWeb.Text = _editableOfficeEquipment.WebUrl;
                cbFile.SelectedItem = _konfigKcDB.FilesSoftwares.FirstOrDefault(os => os.FileID == _editableOfficeEquipment.FileID);
            }
        }

        private void LoadFiles()
        {
            cbFile.ItemsSource = _konfigKcDB.FilesSoftwares.ToList();
            cbFile.SelectedValuePath = "FileID";
            cbFile.DisplayMemberPath = "FileName";
            cbFile.SelectedValue = _editableOfficeEquipment.FileID;
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

            if (_editableOfficeEquipment.OfficeEquipmentID == 0)
            {
                var newOfficeEquip = new OfficeEquipment
                {
                    NameOfficeEquipment = tbName.Text,
                    WebUrl = tbWeb.Text,
                    FileID = prog.FileID
                };

                _konfigKcDB.OfficeEquipments.Add(newOfficeEquip);
            }
            else
            {
                _originalOfficeEquipment.NameOfficeEquipment = _editableOfficeEquipment.NameOfficeEquipment;
                _originalOfficeEquipment.WebUrl = _editableOfficeEquipment.WebUrl;
                _originalOfficeEquipment.FileID = prog.FileID;
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
            return _konfigKcDB.OfficeEquipments.Any(s => s.NameOfficeEquipment == _editableOfficeEquipment.NameOfficeEquipment && s.OfficeEquipmentID != _editableOfficeEquipment.OfficeEquipmentID);
        }

        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            var addEditFileWindow = new AddEditFileWindow(null, _konfigKcDB);
            addEditFileWindow.ShowDialog();
            LoadFiles();
        }
    }
}
