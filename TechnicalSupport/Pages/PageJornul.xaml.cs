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
    /// Логика взаимодействия для PageJornul.xaml
    /// </summary>
    public partial class PageJornul : Page
    {
        ApplicationContext KonfigKc;

        
        public PageJornul()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            listViewReq.ItemsSource=KonfigKc.Requests.ToList();
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
            if (listViewReq.SelectedItem != null)
            {
               Request selectedReq= (Request)listViewReq.SelectedItem;
                EditPageJornul editPageJornul =new EditPageJornul(selectedReq);
                
                NavigationService.Navigate(editPageJornul);
            }
        }
        private void UpdateProduct()
        {
            var curProduct = KonfigKc.SoftwarePositions.ToList();
         

        }
        private void Page_IsVis(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (Visibility == Visibility.Visible)
            {
                KonfigKc.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                listViewReq.ItemsSource = KonfigKc.SoftwarePositions.ToList();
            }

        }

        private void Create_Pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
