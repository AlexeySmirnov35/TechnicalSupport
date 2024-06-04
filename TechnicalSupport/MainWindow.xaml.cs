using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.Pages;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
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
            else
            {
                MasterGlavWindow masterGlavWindow1 = new MasterGlavWindow(userObj);
                MainWindow mainWindow1 = new MainWindow();

                switch (userObj.RoleID)
                {
                    case 1:
                        MessageBox.Show($"Добро пожаловать, {userObj.Surname}!");
                        AdminGlavWindow adminGlavWindow = new AdminGlavWindow(userObj);

                        adminGlavWindow.Show();
                        this.Close();

                        break;
                    case 2:
                        MessageBox.Show($"Добро пожаловать, {userObj.Surname}!");
                        // NavigationService.Navigate(new Uri("Pages/AdminPage.xaml", UriKind.Relative));

                        masterGlavWindow1.Show();
                        this.Close();
                        break;
                    case 3:
                        MessageBox.Show($"Добро пожаловать, {userObj.Surname}!");

                        masterGlavWindow1.Show();
                        this.Close();

                        break;
                    case 4:
                        MessageBox.Show($"Добро пожаловать, {userObj.Surname}!");

                        masterGlavWindow1.Show();
                        this.Close();
                        break;
                    case 5:
                        MessageBox.Show($"Привет, {userObj.Surname}!");
                        masterGlavWindow1.Show();
                        this.Close();
                        break;
                    case 6:
                        MessageBox.Show($"Привет, {userObj.Surname}!");
                        UserGlavWindow userGlavWindow = new UserGlavWindow(userObj);
                        userGlavWindow.Show();
                        this.Close();
                        break;
                    default:
                        break;
                }
            }






            /* try
             {
                 if (tbLog.Text == "1" && tbPas.Password=="1")
                 {
                   MessageBox.Show("Hello!");
                   NavigationService.Navigate(new Uri("Pages/AdminPage.xaml", UriKind.Relative));

                 }
                 else
                 {
                     MessageBox.Show("Не верный логин или пароль");
                 }
             }
             catch(Exception ex)
             {
                 MessageBox.Show("Net");
             }*/




        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
