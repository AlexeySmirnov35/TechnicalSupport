using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для SoftInfoPage.xaml
    /// </summary>

    public partial class SoftInfoPage : Page
    {
        private Software _software=new Software();
        ApplicationContext KonfigKc;


        public SoftInfoPage(Software selectedsoftware)
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            if (selectedsoftware != null)
            {
                _software = selectedsoftware;
            }
            
            DataContext = _software;
            cbFile.ItemsSource=KonfigKc.FilesSoftwares.ToList();
            cbFile.Items.Refresh();

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (Uri.TryCreate(e.Uri.ToString(), UriKind.Absolute, out Uri uriResult))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("chrome.exe", _software.WebUrl));

                e.Handled = true;
            }
            else
            {
                Debug.WriteLine("Некорректный URL");
            }
        }

        private void Btn_Save_Soft_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_software.SoftwareName))
                errors.AppendLine("Укажите название программы!");
                   
            
            if (!Uri.TryCreate(_software.WebUrl, UriKind.Absolute, out var uriResult) || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                errors.AppendLine("Укажите корректную ссылку на документацию!");

            

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var prog = cbFile.SelectedItem as FilesSoftware;
            var dbContext = KonfigKc;

            var isDuplicate = dbContext.Softwares.Any(s => s.SoftwareName == _software.SoftwareName && s.SoftwareID != _software.SoftwareID);

            if (isDuplicate)
            {
                MessageBox.Show("Такая запись существует");
                return;
            }

            if (_software.SoftwareID == 0)
            {
                var newSoftware = new Software
                {
                    SoftwareName = tbName.Text,
                    WebUrl = tbWeb.Text,
                    FileID=prog.FileID
                };

                dbContext.Softwares.Add(newSoftware);
            }

            try
            {
                dbContext.SaveChanges();
                MessageBox.Show("Успешно сохранено");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }



        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/FilePage.xaml", UriKind.Relative));
            
        }

        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbFile.ItemsSource = KonfigKc.FilesSoftwares.ToList();
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
