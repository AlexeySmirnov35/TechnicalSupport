using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AdminUserWindow.xaml
    /// </summary>
    public partial class AdminUserWindow : Window
    {
        private readonly ApplicationContext _konfigKc;
        private readonly User _originalUser;
        private readonly User _editableUser;

        public AdminUserWindow(User user, ApplicationContext konfigKc)
        {
            InitializeComponent();
            _konfigKc = konfigKc;
            _originalUser = user ?? new User();
            _editableUser = new User
            {
                UserID = _originalUser.UserID,
                Cabinet = _originalUser.Cabinet,
                NumberPhone = _originalUser.NumberPhone,
                Surname = _originalUser.Surname,
                Firstname = _originalUser.Firstname,
                Patranomic = _originalUser.Patranomic,
                DepartmentID = _originalUser.DepartmentID,
                PositionsID = _originalUser.PositionsID,
                RoleID = _originalUser.RoleID,
                Login = _originalUser.Login,
                Password = _originalUser.Password
            };
            DataContext = _editableUser;

            LoadComboBoxData();

            if (user != null)
            {
                tbName.Text = _editableUser.Surname;
                tbSurname.Text = _editableUser.Firstname;
                tbNumber.Text = _editableUser.NumberPhone;
                tbCab.Text = _editableUser.Cabinet;
                tbPAt.Text = _editableUser.Patranomic;
                tbLogin.Text = _editableUser.Login;
                tbPassword.Password = _editableUser.Password;
                cbRole.SelectedValue = _editableUser.RoleID;
                cbDepar.SelectedValue = _editableUser.DepartmentID;
                cbPosir.SelectedValue = _editableUser.PositionsID;
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

            if (_editableUser.UserID == 0)
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
                    Login = tbLogin.Text,
                    Password = tbPassword.Password
                };

                _konfigKc.Users.Add(newUser);
            }
            else
            {
                _originalUser.Surname = _editableUser.Surname;
                _originalUser.Firstname = _editableUser.Firstname;
                _originalUser.NumberPhone = _editableUser.NumberPhone;
                _originalUser.Cabinet = _editableUser.Cabinet;
                _originalUser.RoleID = (cbRole.SelectedItem as Role)?.RoleID ?? 0;
                _originalUser.DepartmentID = (cbDepar.SelectedItem as Department)?.DepartmentID ?? 0;
                _originalUser.PositionsID = (cbPosir.SelectedItem as Position)?.PositionID ?? 0;
                _originalUser.Patranomic = _editableUser.Patranomic;
                _originalUser.Login = tbLogin.Text;
                _originalUser.Password = tbPassword.Password;
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

            if (string.IsNullOrWhiteSpace(tbCab.Text))
                errors.AppendLine("Укажите кабинет!");

            if (cbDepar.SelectedItem == null)
                errors.AppendLine("Выберите отдел!");

            if (cbPosir.SelectedItem == null)
                errors.AppendLine("Выберите должность!");

            if (cbRole.SelectedItem == null)
                errors.AppendLine("Выберите роль!");

            if (string.IsNullOrWhiteSpace(tbLogin.Text) || tbLogin.Text.Length < 8)
                errors.AppendLine("Логин должен быть не менее 8 символов!");

            if (string.IsNullOrWhiteSpace(tbPassword.Password) || !IsValidPassword(tbPassword.Password))
                errors.AppendLine("Пароль должен быть не менее 8 символов и содержать хотя бы одну заглавную букву, одну цифру и один специальный символ: ! $ % @ _ ? *");

            return errors;
        }

        private bool IsValidPassword(string password)
        {
            // Пароль должен быть не менее 8 символов, содержать хотя бы одну заглавную букву, одну цифру и один специальный символ
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   Regex.IsMatch(password, @"!$%@_?*");
        }
    }
}
