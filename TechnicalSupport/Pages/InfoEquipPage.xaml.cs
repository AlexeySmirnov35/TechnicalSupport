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
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для InfoEquipPage.xaml
    /// </summary>
    public partial class InfoEquipPage : Page
    {
        private SoftwarePosition _software = new SoftwarePosition();
        private byte[] fileContent = null;
        private FilesSoftware selectedFile = null;
        ApplicationContext KonfigKc;
        private int currentPage = 1;
        private const int PageSize = 10;
        public InfoEquipPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            //  softwareListView.ItemsSource = KonfigKc.SoftwarePositions.ToList();
            //cbSoft.ItemsSource = KonfigKc.Softwares.ToList();
            //cbLis.ItemsSource = KonfigKc.LicensiaInfos.ToList();
            //  cbPosir.ItemsSource=KonfigKc.Positions.ToList();

            LoadDepartments();
            DisplayPage();


        }
    



        private void LoadDepartments()
        {
            softwareListView.ItemsSource = KonfigKc.PositionOfficeEquips.ToList();
        }


        private void DisplayPage()
        {
            var departments = KonfigKc.PositionOfficeEquips
                .OrderBy(d => d.PositOffiiceID)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
            .ToList();

            softwareListView.ItemsSource = departments;

            PageInfo.Text = $"Страница {currentPage} из {Math.Ceiling((double)KonfigKc.PositionOfficeEquips.Count() / PageSize)}";
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayPage();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < (KonfigKc.PositionOfficeEquips.Count() + PageSize - 1) / PageSize)
            {
                currentPage++;
                DisplayPage();
            }
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadDepartments();
            DisplayPage();
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            var selectedSoftwareList = (sender as Button).DataContext as PositionOfficeEquip;

            if (MessageBox.Show($"Вы действительно хотите удалить элемент!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var dbContext = KonfigKc;

                   
                        dbContext.PositionOfficeEquips.Remove(selectedSoftwareList);
                    

                    dbContext.SaveChanges();
                    MessageBox.Show("Удаление прошло успешно");
                    LoadDepartments();
                    DisplayPage();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении элементов: {ex.Message}");
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var v = (sender as System.Windows.Controls.Button).DataContext as PositionOfficeEquip;
            EditEquipWindow editEquipWindow = new EditEquipWindow(v, KonfigKc);
            editEquipWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
        }

        private void Btn_AddFile(object sender, RoutedEventArgs e)
        {
           AddEquipWindow addEquipWindow = new AddEquipWindow();
            addEquipWindow.ShowDialog();
            LoadDepartments();
            DisplayPage();
        }
    }
}
