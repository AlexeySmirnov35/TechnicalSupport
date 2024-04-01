
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
                    switch (userObj.RoleID)
                    {
                        case 1:
                            MessageBox.Show("Hello Админ!");
                            NavigationService.Navigate(new Uri("Pages/AddFilePage.xaml", UriKind.Relative));
                            break;
                        case 2:
                            MessageBox.Show("Hello Диспетчер");
                            NavigationService.Navigate(new Uri("Pages/AdminPage.xaml", UriKind.Relative));
                            break;
                        case 3:
                            MessageBox.Show("Hello! Исполнитель");
                            NavigationService.Navigate(new Uri("Pages/AdminPage.xaml", UriKind.Relative));
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
            NavigationService.GoBack();
        }
    }
}
