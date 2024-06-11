using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.Pages;

namespace TechnicalSupport.Pages
{
    public partial class UserPage : Page
    {
        ApplicationContext KonfigKc;

        public UserPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            frmMain.Navigate(new DepartPage());
            
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

        private void Perexod_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new DepartPage());
        }

        private void PerexodRole_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new FilePage());
        }

        private void Perexod1_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new FormPage());
        }

        private void Upravlenie_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate( new StatisticPage());
        }

        private void ProgramnoeObespechenie_Click(object sender, RoutedEventArgs e)
        {
          frmMain.Navigate(new SoftwarePage());
        }

        private void OperatsionnyeSystemy_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new OperSystemPage());     
        }

        private void OborudovanieUpravlenie_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new OfficeEquipPage());
            
        }

        private void CniiSetPodrazdelenie_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate( new DepartPage());
        }

        private void CniiSetDoljnosti_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate( new PageJornul()); 
            
        }

        private void CniiSetRoli_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new RolePage());
        }

        private void BazaZnaniy_Click(object sender, RoutedEventArgs e)
        {
           // AddTab("База Знаний", new Uri("EditPageInformation.xaml", UriKind.Relative));
          //  frmMain.Navigate(new EditPageInformation());
           
        }
        private void Faily_Click(object sender, RoutedEventArgs e)
        {
            //  AddTab("Файлы", new Uri("FilePage.xaml", UriKind.Relative));
            frmMain.Navigate(new FilePage());
        }

        private void Zayavki_Click(object sender, RoutedEventArgs e)
        {
            // AddTab("База Знаний", new Uri("FormPage.xaml", UriKind.Relative));
            frmMain.Navigate(new FormPage());
        }
        private void AddTab(string header, Uri pageUri)
        {
           /* foreach (StackPanel tabPanel in Table.Children.OfType<StackPanel>())
            {
                Button existingTabButton = tabPanel.Children.OfType<Button>().FirstOrDefault();
                if (existingTabButton != null && existingTabButton.Content.ToString() == header)
                {
                    frmMain.Source = pageUri;
                    return;
                }
            }

            StackPanel newTabPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 5, 0)
            };

            var newTabButton = new Button
            {
                Content = header,
                Background = (SolidColorBrush)FindResource("Button.Static.Background"),
                Foreground = Brushes.White
            };

            newTabButton.Click += (sender, e) =>
            {
                frmMain.Source = pageUri;
            };

            var closeButton = new Button
            {
                Content = "X",
                Background = Brushes.Red,
                Foreground = Brushes.White,
                Width = 20,
                Height = 20,
                Margin = new Thickness(5, 0, 0, 0)
            };

            closeButton.Click += (sender, e) =>
            {
                Table.Children.Remove(newTabPanel);
                if (Table.Children.Count > 0)
                {
                    StackPanel firstTabPanel = Table.Children[0] as StackPanel;
                    Button firstTabButton = firstTabPanel.Children[0] as Button;
                    frmMain.Source = new Uri(firstTabButton.Tag.ToString(), UriKind.Relative);
                }
                else
                {
                    frmMain.Content = null;
                }
            };

            newTabButton.Tag = pageUri.ToString();

            newTabPanel.Children.Add(newTabButton);
            newTabPanel.Children.Add(closeButton);

            Table.Children.Add(newTabPanel);

            frmMain.Source = pageUri;*/
        }



        
    }
}
