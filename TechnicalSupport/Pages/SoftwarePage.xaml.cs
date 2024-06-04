using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для SoftwarePage.xaml
    /// </summary>
    public partial class SoftwarePage : Page
    {
        ApplicationContext KonfigKc;


        public SoftwarePage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            listview.ItemsSource=KonfigKc.Softwares.ToList();
            
        }

        private void Tbox_Search(object sender, TextChangedEventArgs e)
        {

        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview.SelectedItem != null)
            {
                Software selectedSoftware = (Software)listview.SelectedItem;
                SoftInfoPage editSoftwarePage = new SoftInfoPage(selectedSoftware);
                NavigationService.Navigate(editSoftwarePage);
            }
        }

        private void Add_Soft_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new SoftInfoPage(null));
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

      

        private void Btn_Del(object sender, RoutedEventArgs e)
        {
            var softToDelete = listview.SelectedItems.Cast<Software>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить эти {softToDelete.Count()} элемента!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var dbContext = KonfigKc;

                foreach (var softw in softToDelete)
                {
                    if (!dbContext.SoftwarePositions.Any(item => item.SoftwareID == softw.SoftwareID))
                    {
                        dbContext.Softwares.Remove(softw);
                    }
                    else
                    {
                        MessageBox.Show($"Файл {softw.SoftwareName} используется в других таблицах и не может быть удален.");
                    }
                }

                dbContext.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                listview.ItemsSource = dbContext.FilesSoftwares.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
