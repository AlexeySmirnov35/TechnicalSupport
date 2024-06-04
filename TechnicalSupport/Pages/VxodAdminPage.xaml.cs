
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для VxodAdminPage.xaml
    /// </summary>
    public partial class VxodAdminPage : Page
    {
        ApplicationContext KonfigKc;

       
        public VxodAdminPage()
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
                            MessageBox.Show("Hello Админ}!");
                            AdminGlavWindow adminGlavWindow = new AdminGlavWindow(userObj);

                            adminGlavWindow.Show();
                            mainWindow1.Close();

                        break;
                        case 2:
                            MessageBox.Show("Hello Диспетчер");
                           // NavigationService.Navigate(new Uri("Pages/AdminPage.xaml", UriKind.Relative));
                       
                        masterGlavWindow1.Show();
                        mainWindow1.Close();
                        break;
                        case 3:
                            MessageBox.Show("Hello! Исполнитель");
                       
                        masterGlavWindow1.Show();
                        mainWindow1.Close();

                        break;
                        case 4:
                        MessageBox.Show("Hello! Исполнитель");
                     
                        masterGlavWindow1.Show();
                        mainWindow1.Close();
                        break;
                         case 5:
                        MessageBox.Show("Hello! Исполнитель");
                        masterGlavWindow1.Show();
                        mainWindow1.Close();
                        break;
                       case 6:
                        MessageBox.Show("Hello! Исполнитель");
                        UserGlavWindow userGlavWindow=new UserGlavWindow(userObj);
                        userGlavWindow.Show();
                        mainWindow1.Close(); 
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
           MainWindow mainWindow = new MainWindow();
            mainWindow.Close();
        }
    }
}
