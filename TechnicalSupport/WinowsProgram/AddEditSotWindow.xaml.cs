// File: AddEditSotWindow.xaml.cs
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    public partial class AddEditSotWindow : Window
    {
        private readonly ApplicationContext _konfigKc;
        private readonly User _originalUser;
        private readonly User _editableUser;

        public AddEditSotWindow(User user, ApplicationContext konfigKc)
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

            if (user != null)
            {
                pbPassword.Password = _editableUser.Password;
                pbConfirmPassword.Password = _editableUser.Password;
            }
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = ValidateInputs();

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var userToUpdate = _konfigKc.Users.FirstOrDefault(u => u.UserID == _originalUser.UserID);
            if (userToUpdate != null)
            {
                userToUpdate.Password = pbPassword.Password;

                try
                {
                    _konfigKc.SaveChanges();
                    MessageBox.Show("Пароль успешно изменен");
                    MasterGlavWindow masterGlavWindow = new MasterGlavWindow(userToUpdate);
                    switch (userToUpdate.RoleID)
                    {
                        case 1:
                            MessageBox.Show($"Добро пожаловать, {userToUpdate.Surname}!");
                            AdminGlavWindow adminGlavWindow = new AdminGlavWindow(userToUpdate);
                            adminGlavWindow.Show();
                            break;
                        case 2:
                            MessageBox.Show($"Добро пожаловать, {userToUpdate.Surname}!");
                            masterGlavWindow.Show();
                            break;
                        case 3:
                            MessageBox.Show($"Добро пожаловать, {userToUpdate.Surname}!");
                            masterGlavWindow.Show();
                            break;
                        case 4:
                            MessageBox.Show($"Добро пожаловать, {userToUpdate.Surname}!");
                            masterGlavWindow.Show();
                            break;
                        case 5:
                            MessageBox.Show($"Добро пожаловать, {userToUpdate.Surname}!");
                            masterGlavWindow.Show();
                            break;
                        case 6:
                            MessageBox.Show($"Добро пожаловать, {userToUpdate.Surname}!");
                            UserGlavWindow userGlavWindow = new UserGlavWindow(userToUpdate);
                            masterGlavWindow.Show();
                            break;
                        default:
                            break;
                    }
                    this.Close();
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

        private StringBuilder ValidateInputs()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(pbPassword.Password) || !IsValidPassword(pbPassword.Password))
                errors.AppendLine("Пароль должен быть не менее 8 символов и содержать хотя бы одну заглавную букву, одну цифру и один специальный символ: ! $ % @ _ ? *");

            if (pbPassword.Password != pbConfirmPassword.Password)
                errors.AppendLine("Пароли не совпадают!");

            return errors;
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   Regex.IsMatch(password, @"[!$%@_?*]");
        }

        private void BtnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            if (tbPassword.Visibility == Visibility.Collapsed)
            {
                tbPassword.Visibility = Visibility.Visible;
                tbPassword.Text = pbPassword.Password;
                pbPassword.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-invisible-96.png"));
            }
            else
            {
                pbPassword.Visibility = Visibility.Visible;
                pbPassword.Password = tbPassword.Text;
                tbPassword.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-eye-96.png"));
            }
        }

        private void BtnShowPasswordDouble_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            if (tbConPassword.Visibility == Visibility.Collapsed)
            {
                tbConPassword.Visibility = Visibility.Visible;
                tbConPassword.Text = pbConfirmPassword.Password;
                pbConfirmPassword.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-invisible-96.png"));
            }
            else
            {
                pbConfirmPassword.Visibility = Visibility.Visible;
                pbConfirmPassword.Password = tbConPassword.Text;
                tbConPassword.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-eye-96.png"));
            }
        }
    }
}
