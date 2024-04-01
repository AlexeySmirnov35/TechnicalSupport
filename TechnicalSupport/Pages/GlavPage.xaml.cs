
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
    /// Логика взаимодействия для GlavPage.xaml
    /// </summary>
    public partial class GlavPage : Page
    {
        public GlavPage()
        {
            InitializeComponent();
        }

        private void Btn_User_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/UserPage.xaml", UriKind.Relative));


        }
        private void UserWindow_Closed(object sender, EventArgs e)
        {
            
            Window.GetWindow(this).Close();
        }
        private void Btn_Admin_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("Pages/VxodAdminPage.xaml", UriKind.Relative));
        }
    }
}
