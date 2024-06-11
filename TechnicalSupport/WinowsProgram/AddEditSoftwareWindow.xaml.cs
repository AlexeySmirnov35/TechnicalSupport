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
        private readonly Software _software;

        public AddEditSoftwareWindow(Software software = null)
        {
            InitializeComponent();
            _konfigKcDB = new ApplicationContext();
            _software = software ?? new Software();
            DataContext = _software;
            LoadComboBoxData();

            if (_software.SoftwareID != 0)
            {
                tbName.Text = _software.SoftwareName;
                tbWeb.Text = _software.WebUrl;
                cbFile.SelectedItem = _konfigKcDB.FilesSoftwares.FirstOrDefault(fs => fs.FileID == _software.FileID);
                cbLis.SelectedItem = _konfigKcDB.LicensiaInfoes.FirstOrDefault(li => li.LicenseID == _software.LicenseID);
                cbTypeSoft.SelectedItem = _konfigKcDB.TypeSofwares.FirstOrDefault(ts => ts.TypeSofwareID == _software.TypeSofwareID);
            }
        }

        private void LoadComboBoxData()
        {
            cbFile.ItemsSource = _konfigKcDB.FilesSoftwares.ToList();
            cbLis.ItemsSource = _konfigKcDB.LicensiaInfoes.ToList();
            cbTypeSoft.ItemsSource = _konfigKcDB.TypeSofwares.ToList();
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
                if (_software.SoftwareID == 0)
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
            return _konfigKcDB.Softwares.Any(s => s.SoftwareName == tbName.Text && s.SoftwareID != _software.SoftwareID);
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
            _software.SoftwareName = tbName.Text;
            _software.WebUrl = tbWeb.Text;
            _software.FileID = prog.FileID;
            _software.LicenseID = (cbLis.SelectedItem as LicensiaInfo)?.LicenseID ?? 0;
            _software.TypeSofwareID = (cbTypeSoft.SelectedItem as TypeSofware)?.TypeSofwareID ?? 0;
        }

        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            AddEditFileWindow addEditFileWindow = new AddEditFileWindow(null);
            addEditFileWindow.ShowDialog();
            LoadComboBoxData();
        }

        private void Win_Load(object sender, RoutedEventArgs e)
        {

        }
    }
}
