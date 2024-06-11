using System;
using System.Linq;
using System.Text;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AdminUserWindow.xaml
    /// </summary>
    public partial class AdminUserWindow : Window
    {
        private User _user;
        private readonly ApplicationContext _konfigKc;

        public AdminUserWindow(User user)
        {
            InitializeComponent();
            _konfigKc = new ApplicationContext();
            _user = user ?? new User();
            DataContext = _user;

            LoadComboBoxData();

            if (user != null)
            {
                tbName.Text = user.Surname;
                tbSurname.Text = user.Firstname;
                tbNumber.Text = user.NumberPhone;
                tbCab.Text = user.Cabinet;
                tbPAt.Text = user.Patranomic;
                cbRole.SelectedItem = _konfigKc.Roles.FirstOrDefault(r => r.RoleID == user.RoleID);
                cbDepar.SelectedItem = _konfigKc.Departments.FirstOrDefault(d => d.DepartmentID == user.DepartmentID);
                cbPosir.SelectedItem = _konfigKc.Positions.FirstOrDefault(p => p.PositionID == user.PositionsID);
            }
        }

        private void LoadComboBoxData()
        {
            cbPosir.ItemsSource = _konfigKc.Positions.ToList();
            cbDepar.ItemsSource = _konfigKc.Departments.ToList();
            cbRole.ItemsSource = _konfigKc.Roles.ToList();
        }

        private void Create_Req_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = ValidateInputs();

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_user.UserID == 0) 
            {
                User newUser = new User
                {
                    Cabinet = tbCab.Text,
                    NumberPhone = tbNumber.Text,
                    Surname = tbName.Text,
                    Firstname = tbSurname.Text,
                    Patranomic = tbPAt.Text,
                    DepartmentID = (cbDepar.SelectedItem as Department)?.DepartmentID ?? 0,
                    PositionsID = (cbPosir.SelectedItem as Position)?.PositionID ?? 0,
                    RoleID = (cbRole.SelectedItem as Role)?.RoleID ?? 0,
                };

                _konfigKc.Users.Add(newUser);
            }
            else 
            {
                _user.Surname = tbName.Text;
                _user.Firstname = tbSurname.Text;
                _user.NumberPhone = tbNumber.Text;
                _user.Cabinet = tbCab.Text;
                _user.RoleID = (cbRole.SelectedItem as Role)?.RoleID ?? 0;
                _user.DepartmentID = (cbDepar.SelectedItem as Department)?.DepartmentID ?? 0;
                _user.PositionsID = (cbPosir.SelectedItem as Position)?.PositionID ?? 0;
                _user.Patranomic = tbPAt.Text;
            }

            try
            {
                _konfigKc.SaveChanges(); 
                MessageBox.Show("Успешно сохранено");
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
                errors.AppendLine("Укажите фамилию!");

            if (string.IsNullOrWhiteSpace(tbSurname.Text))
                errors.AppendLine("Укажите имя!");

            if (string.IsNullOrWhiteSpace(tbNumber.Text))
                errors.AppendLine("Укажите номер телефона!");
            if (cbDepar.SelectedItem == null)
                    errors.AppendLine("Номер должен содержать только цифры!!");
                if (cbPosir.SelectedItem == null)
                    errors.AppendLine("Укажите должность!");
            if (string.IsNullOrWhiteSpace(tbCab.Text))
                errors.AppendLine("Укажите кабинет!");

            if (cbDepar.SelectedItem == null)
                errors.AppendLine("Выберите отдел!");

            if (cbPosir.SelectedItem == null)
                errors.AppendLine("Выберите должность!");

            if (cbRole.SelectedItem == null)
                errors.AppendLine("Выберите роль!");

            return errors;
        }
    }
}
