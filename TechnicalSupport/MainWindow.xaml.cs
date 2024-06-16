using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport
{
    public partial class MainWindow : Window
    {
        ApplicationContext KonfigKc;

        public MainWindow()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
        }

        private void Btn_Vxod(object sender, RoutedEventArgs e)
        {
            var userObj = KonfigKc.Users.FirstOrDefault(x => x.Login == tbLog.Text && x.Password == tbPas.Password);
            if (userObj == null)
            {
                MessageBox.Show("Не верный логин или пароль");
                return;
            }

            if (userObj.Password == "Password_123")
            {
                MessageBox.Show("Требуется смена пароля");
                AddEditSotWindow addEditSotWindow = new AddEditSotWindow(userObj, KonfigKc);
                addEditSotWindow.ShowDialog();
               
                return;
            }

            OpenUserWindow(userObj);
        }

        private void OpenUserWindow(User user)
        {
            MasterGlavWindow masterGlavWindow = new MasterGlavWindow(user);
            switch (user.RoleID)
            {
                case 1:
                    MessageBox.Show($"Добро пожаловать, {user.Surname}!");
                    AdminGlavWindow adminGlavWindow = new AdminGlavWindow(user);
                    adminGlavWindow.Show();
                    this.Close();
                    break;
                case 2:
                    MessageBox.Show($"Добро пожаловать, {user.Surname}!");
                    masterGlavWindow.Show();
                    this.Close();
                    break;
                case 3:
                    MessageBox.Show($"Добро пожаловать, {user.Surname}!");
                    masterGlavWindow.Show();
                    this.Close();
                    break;
                case 4:
                    MessageBox.Show($"Добро пожаловать, {user.Surname}!");
                    masterGlavWindow.Show();
                    this.Close();
                    break;
                case 5:
                    MessageBox.Show($"Добро пожаловать, {user.Surname}!");
                    masterGlavWindow.Show();
                    this.Close();
                    break;
                case 6:
                    MessageBox.Show($"Добро пожаловать, {user.Surname}!");
                    UserGlavWindow userGlavWindow = new UserGlavWindow(user);
                    userGlavWindow.Show();
                    this.Close();
                    break;
                default:
                    break;
            }
            this.Close();
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            if (tbPassword.Visibility == Visibility.Collapsed)
            {
                tbPassword.Visibility = Visibility.Visible;
                tbPassword.Text = tbPas.Password;
                tbPas.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-invisible-96.png"));
            }
            else
            {
                tbPas.Visibility = Visibility.Visible;
                tbPas.Password = tbPassword.Text;
                tbPassword.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-eye-96.png"));
            }
        }
    }
}