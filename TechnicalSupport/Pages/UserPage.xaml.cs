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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        ApplicationContext KonfigKc;

       
        public UserPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
        }

        private void Btn_AddRequest(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/FormPage.xaml", UriKind.Relative));
        }

        private void Btn_Jornul_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/PageJornul.xaml", UriKind.Relative));
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/UserSoft.xaml", UriKind.Relative));
        }
    }
}
