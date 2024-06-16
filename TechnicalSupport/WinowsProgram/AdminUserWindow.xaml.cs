using PdfSharp.Pdf.Content.Objects;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    public partial class AdminUserWindow : Window
    {
        private readonly ApplicationContext _konfigKc;
        private readonly User _originalUser;
        private readonly User _editableUser;
        private readonly User _currentAdmin;

        public AdminUserWindow(User user, ApplicationContext konfigKc, User currentAdmin)
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
            _currentAdmin = currentAdmin;
            DataContext = _editableUser;

            LoadComboBoxData();

            if (user != null)
            {
                tbSurname.Text = _editableUser.Surname;
                tbFirstname.Text = _editableUser.Firstname;
                tbNumber.Text = _editableUser.NumberPhone;
                tbCab.Text = _editableUser.Cabinet;
                tbPatronymic.Text = _editableUser.Patranomic;
                tbLogin.Text = _editableUser.Login;
                cbRole.SelectedValue = _editableUser.RoleID;
                cbDepartment.SelectedValue = _editableUser.DepartmentID;
                cbPosition.SelectedValue = _editableUser.PositionsID;
            }
        }

        private void LoadComboBoxData()
        {
            cbPosition.ItemsSource = _konfigKc.Positions.ToList();
            cbPosition.SelectedValuePath = "PositionID";
            cbPosition.DisplayMemberPath = "PositionName";
            cbPosition.SelectedValue = _editableUser.PositionsID;

            cbDepartment.ItemsSource = _konfigKc.Departments.ToList();
            cbDepartment.SelectedValuePath = "DepartmentID";
            cbDepartment.DisplayMemberPath = "DepartmentName";
            cbDepartment.SelectedValue = _editableUser.DepartmentID;

            cbRole.ItemsSource = _konfigKc.Roles.ToList();
            cbRole.SelectedValuePath = "RoleID";
            cbRole.DisplayMemberPath = "RoleName";
            cbRole.SelectedValue = _editableUser.RoleID;
        }

        private void Create_Req_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = ValidateInputs();

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Check if login already exists
            string newLogin = tbLogin.Text;
            bool loginExists = _konfigKc.Users.Any(u => u.Login == newLogin && u.UserID != _editableUser.UserID);
            if (loginExists)
            {
                MessageBox.Show("Логин уже существует. Пожалуйста, выберите другой логин.");
                return;
            }

            if (_editableUser.UserID == 0)
            {
                User newUser = new User
                {
                    Cabinet = tbCab.Text,
                    NumberPhone = tbNumber.Text,
                    Surname = tbSurname.Text,
                    Firstname = tbFirstname.Text,
                    Patranomic = tbPatronymic.Text,
                    DepartmentID = (cbDepartment.SelectedItem as Department)?.DepartmentID ?? 0,
                    PositionsID = (cbPosition.SelectedItem as Position)?.PositionID ?? 0,
                    RoleID = (cbRole.SelectedItem as Role)?.RoleID ?? 0,
                    Login = newLogin,
                    Password = "Password_123"
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
                _originalUser.DepartmentID = (cbDepartment.SelectedItem as Department)?.DepartmentID ?? 0;
                _originalUser.PositionsID = (cbPosition.SelectedItem as Position)?.PositionID ?? 0;
                _originalUser.Patranomic = _editableUser.Patranomic;
                _originalUser.Login = newLogin;
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

            if (string.IsNullOrWhiteSpace(tbSurname.Text))
                errors.AppendLine("Укажите фамилию!");

            if (string.IsNullOrWhiteSpace(tbFirstname.Text))
                errors.AppendLine("Укажите имя!");

            if (string.IsNullOrWhiteSpace(tbNumber.Text))
                errors.AppendLine("Укажите номер телефона!");

            if (string.IsNullOrWhiteSpace(tbCab.Text))
                errors.AppendLine("Укажите кабинет!");

            if (cbDepartment.SelectedItem == null)
                errors.AppendLine("Выберите отдел!");

            if (cbPosition.SelectedItem == null)
                errors.AppendLine("Выберите должность!");

            if (cbRole.SelectedItem == null)
                errors.AppendLine("Выберите роль!");

            if (string.IsNullOrWhiteSpace(tbLogin.Text) || tbLogin.Text.Length < 8)
                errors.AppendLine("Логин должен быть не менее 8 символов!");

            return errors;
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   Regex.IsMatch(password, @"[!$%@_?*]");
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var passwordDialog = new AdminPasswordDialog();
            if (passwordDialog.ShowDialog() == true)
            {
                if (passwordDialog.AdminPassword == _currentAdmin.Password)
                {
                    ResetUserPassword();
                }
                else
                {
                    MessageBox.Show("Неверный пароль администратора.");
                }
            }
        }

        private void ResetUserPassword()
        {
            var userToUpdate = _konfigKc.Users.FirstOrDefault(u => u.UserID == _originalUser.UserID);
            if (userToUpdate != null)
            {
                userToUpdate.Password = "Password_123";

                try
                {
                    _konfigKc.SaveChanges();
                    MessageBox.Show("Пароль успешно сброшен на Password_123");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }
    }
}
