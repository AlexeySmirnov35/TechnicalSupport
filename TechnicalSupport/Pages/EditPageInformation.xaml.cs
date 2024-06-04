using Microsoft.Win32;
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

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPageInformation.xaml
    /// </summary>
    public partial class EditPageInformation : Page
    {
        ApplicationContext KonfigKc;


        private SoftwarePosition _software=new SoftwarePosition();
        public EditPageInformation(SoftwarePosition selectSoftware)
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
          if (selectSoftware != null)
            {
                _software = selectSoftware;
            }
            cbAllProg.ItemsSource=KonfigKc.Softwares.ToList();
            //cbLinc.ItemsSource=KonfigKc.LicensiaInfos.ToList();
            DataContext = _software;
            tbVProg.Text=_software.Software.SoftwareName.ToString();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            var prog = cbAllProg.SelectedItem as Software;
            var linc = cbLinc.SelectedItem as LicensiaInfo;

            StringBuilder errors = new StringBuilder();

            if (prog == null)
                errors.AppendLine("Выберите программу для замены");

            if (linc == null)
                errors.AppendLine("Выберите необходимость лицензии");

            var dbContext = KonfigKc;
            var isDuplicate = dbContext.SoftwarePositions
                .Any(sp => sp.PositionID == _software.PositionID && sp.SoftwareID == prog.SoftwareID);

            if (isDuplicate)
                errors.AppendLine("Такая запись существует");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_software.PositionID == 0)
            {
                var softpos = new SoftwarePosition
                {
                    SoftwareID = prog?.SoftwareID ?? 0,
                    //LicenseID = linc?.LicenseID ?? 0
                };

                dbContext.SoftwarePositions.Add(softpos);
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


        private void Tbox_Search(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Image_Load(object sender, RoutedEventArgs e)
        {
            

        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
