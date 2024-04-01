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
    /// Логика взаимодействия для InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        private SoftwarePosition _software=new SoftwarePosition();
        ApplicationContext KonfigKc;

      
        public InfoPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            softwareListView.ItemsSource = KonfigKc.SoftwarePositions.ToList();
            cbSoft.ItemsSource = KonfigKc.Softwares.ToList();
            //cbLis.ItemsSource = KonfigKc.LicensiaInfos.ToList();
            cbPosir.ItemsSource=KonfigKc.Positions.ToList();
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (softwareListView.SelectedItem != null)
            {
                SoftwarePosition selectedSoftware = (SoftwarePosition)softwareListView.SelectedItem;
                EditPageInformation editSoftwarePage = new EditPageInformation(selectedSoftware);
                NavigationService.Navigate(editSoftwarePage);
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Btn_Create(object sender, RoutedEventArgs e)
        {
            var prog = cbSoft.SelectedItem as Software;
            var posit = cbPosir.SelectedItem as Position;

            if (prog == null || posit == null)
            {
                MessageBox.Show("Выберите программу и должность");
                return;
            }

            var dbContext = KonfigKc;

            try
            {
                var newSoftwarePosition = new SoftwarePosition
                {
                    SoftwareID = prog.SoftwareID,
                    PositionID = posit.PositionID,
                    LicenseTreb=1
                    
                };

                // Присваиваем новый объект _software перед добавлением
                _software = newSoftwarePosition;

                dbContext.SoftwarePositions.Add(newSoftwarePosition);
                dbContext.SaveChanges();

                MessageBox.Show("Успешно сохранено");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }


        /* private void Btn_Create(object sender, RoutedEventArgs e)
         {
             var prog = cbSoft.SelectedItem as Software;
            // var linc = cbLis.SelectedItem as LicensiaInfo;
             var posit = cbPosir.SelectedItem as Position;

             if (prog == null || posit == null  )
             {
                 MessageBox.Show("Выберите программу, должность и необходимость лицензии");
                 return;
             }

             var dbContext = KonfigKc;

             try
             {
                 if (_software.PositionID == 0)
                 {
                   /* var isDuplicate = dbContext.SoftwarePositions
                         //.Any(sp => sp.PositionID == posit.PositionID && sp.SoftwareID == prog.SoftwareID);

                     if (isDuplicate)
                     {
                         MessageBox.Show("Такая запись существует");
                         return;
                     }

                     var newSoftwarePosition = new SoftwarePosition
                     {
                         SoftwareID = prog.SoftwareID,
                         //LicenseID = linc.LicenseID,
                         PositionID = posit.PositionID
                     };

                     dbContext.SoftwarePositions.Add(newSoftwarePosition);
                     dbContext.SaveChanges();

                     MessageBox.Show("Успешно сохранено");
                     NavigationService.GoBack();
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
             }
         }*/



        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            var selectedSoftwareList = softwareListView.SelectedItems.Cast<SoftwarePosition>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить эти {selectedSoftwareList.Count()} элемента!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var dbContext = KonfigKc;

                    foreach (var selectedSoftware in selectedSoftwareList)
                    {
                        dbContext.SoftwarePositions.Remove(selectedSoftware);
                    }

                    dbContext.SaveChanges();
                    MessageBox.Show("Удаление прошло успешно");
                    softwareListView.ItemsSource = dbContext.SoftwarePositions.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении элементов: {ex.Message}");
                }
            }
        }
    }
}
