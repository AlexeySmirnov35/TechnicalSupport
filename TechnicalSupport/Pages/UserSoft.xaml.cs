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
    /// Логика взаимодействия для UserSoft.xaml
    /// </summary>
    public partial class UserSoft : Page
    {
        ApplicationContext KonfigKc;

      
        public UserSoft()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            listview.ItemsSource=KonfigKc.Softwares.ToList();
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview.SelectedItem != null)
            {
                Software selectedSoftware = (Software)listview.SelectedItem;
                UserPageSoft userPageSoft = new UserPageSoft(selectedSoftware);
                NavigationService.Navigate(userPageSoft);
                
            }
        }

        private void Tbox_Search(object sender, TextChangedEventArgs e)
        {
            UpdateSoftware();
        }
        private void UpdateSoftware()
        {
            var allSoftware = KonfigKc.Softwares.ToList();
            allSoftware = allSoftware.Where(s => s.SoftwareName.ToLower().Contains(tbox_Search.Text.ToLower())).ToList();
            listview.ItemsSource = allSoftware.OrderBy(s => s.SoftwareName).ToList();
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
