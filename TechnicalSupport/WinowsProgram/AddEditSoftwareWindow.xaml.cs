using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Interaction logic for AddEditSoftwareWindow.xaml
    /// </summary>
    public partial class AddEditSoftwareWindow : Window
    {
        private readonly ApplicationContext _konfigKcDB;
        private readonly Software _originalSoftware;
        private readonly Software _editableSoftware;

        public AddEditSoftwareWindow(Software software, ApplicationContext konfigKcDB)
        {
            InitializeComponent();
            _konfigKcDB = konfigKcDB;
            _originalSoftware = software ?? new Software();
            _editableSoftware = new Software
            {
                SoftwareID = _originalSoftware.SoftwareID,
                SoftwareName = _originalSoftware.SoftwareName,
                WebUrl = _originalSoftware.WebUrl,
                FileID = _originalSoftware.FileID,
                LicenseID = _originalSoftware.LicenseID,
                TypeSofwareID = _originalSoftware.TypeSofwareID
            };
            DataContext = _editableSoftware;
            LoadComboBoxData();

            if (_originalSoftware.SoftwareID != 0)
            {
                tbName.Text = _editableSoftware.SoftwareName;
                tbWeb.Text = _editableSoftware.WebUrl;
                cbFile.SelectedValue = _editableSoftware.FileID;
                cbLis.SelectedValue = _editableSoftware.LicenseID;
                cbTypeSoft.SelectedValue = _editableSoftware.TypeSofwareID;
            }
        }

        private void LoadComboBoxData()
        {
            cbFile.ItemsSource = _konfigKcDB.FilesSoftwares.ToList();
            cbFile.SelectedValuePath = "FileID";
            cbFile.DisplayMemberPath = "FileName";
            cbFile.SelectedValue = _editableSoftware.FileID;

            cbLis.ItemsSource = _konfigKcDB.LicensiaInfoes.ToList();
            cbLis.SelectedValuePath = "LicenseID";
            cbLis.DisplayMemberPath = "LicenseType";
            cbLis.SelectedValue = _editableSoftware.LicenseID;

            cbTypeSoft.ItemsSource = _konfigKcDB.TypeSofwares.ToList();
            cbTypeSoft.SelectedValuePath = "TypeSofwareID";
            cbTypeSoft.DisplayMemberPath = "NameType";
            cbTypeSoft.SelectedValue = _editableSoftware.TypeSofwareID;
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

            try
            {
                if (_editableSoftware.SoftwareID == 0)
                {
                    AddSoftware(prog);
                }
                else
                {
                    UpdateSoftware(prog);
                }

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

            if (cbFile.SelectedItem == null)
                errors.AppendLine("Выберите файл программы!");

            if (cbLis.SelectedItem == null)
                errors.AppendLine("Выберите лицензию!");

            if (cbTypeSoft.SelectedItem == null)
                errors.AppendLine("Выберите тип программы!");

            return errors;
        }

        private bool IsDuplicateRecord()
        {
            return _konfigKcDB.Softwares.Any(s => s.SoftwareName == tbName.Text && s.SoftwareID != _editableSoftware.SoftwareID);
        }

        private void AddSoftware(FilesSoftware prog)
        {
            var newSoftware = new Software
            {
                SoftwareName = tbName.Text,
                WebUrl = tbWeb.Text,
                FileID = prog.FileID,
                LicenseID = (cbLis.SelectedItem as LicensiaInfo)?.LicenseID ?? 0,
                TypeSofwareID = (cbTypeSoft.SelectedItem as TypeSofware)?.TypeSofwareID ?? 0
            };

            _konfigKcDB.Softwares.Add(newSoftware);
        }

        private void UpdateSoftware(FilesSoftware prog)
        {
            _originalSoftware.SoftwareName = _editableSoftware.SoftwareName;
            _originalSoftware.WebUrl = _editableSoftware.WebUrl;
            _originalSoftware.FileID = prog.FileID;
            _originalSoftware.LicenseID = (cbLis.SelectedItem as LicensiaInfo)?.LicenseID ?? 0;
            _originalSoftware.TypeSofwareID = (cbTypeSoft.SelectedItem as TypeSofware)?.TypeSofwareID ?? 0;
        }

        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            var addEditFileWindow = new AddEditFileWindow(null, _konfigKcDB);
            addEditFileWindow.ShowDialog();
            LoadComboBoxData();
        }

        private void Win_Load(object sender, RoutedEventArgs e)
        {
            // Placeholder for any actions on window load
        }
    }
}
