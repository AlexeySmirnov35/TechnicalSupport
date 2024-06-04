using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для InformationPage.xaml
    /// </summary>
    public partial class InformationPage : Page
    {
        ApplicationContext KonfigKc;

     
        public InformationPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            LoadPositions();
        }

        private void LoadPositions()
        {
            var softwarePositions = KonfigKc.SoftwarePositions.ToList();
            var groupedPositions = softwarePositions.GroupBy(sp => sp.PositionID);
            var positionsViewModel = new ObservableCollection<PositionViewModel>();
            foreach (var group in groupedPositions)
            {
                var positionViewModel = new PositionViewModel
                {
                    PositionID = group.Key,
                    Programs = new ObservableCollection<Software>(group.Select(sp => sp.Software)),
                    AllPrograms = new ObservableCollection<Software>(KonfigKc.Softwares.ToList())
                };

                positionsViewModel.Add(positionViewModel);
            }
            positionDataGrid.ItemsSource = positionsViewModel;
           
        }

        private void PositionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox allProgramComboBox = new ComboBox();
            allProgramComboBox.ItemsSource = KonfigKc.Softwares.ToList();
            ComboBox programComboBox = new ComboBox();

            if (positionDataGrid.SelectedItem != null)
            {
                PositionViewModel selectedPosition = (PositionViewModel)positionDataGrid.SelectedItem;
                ObservableCollection<Software> softwareList = new ObservableCollection<Software>(
                from sp in KonfigKc.SoftwarePositions
                where sp.PositionID == selectedPosition.PositionID
                select sp.Software);
                var cbProg = positionDataGrid.Columns[1].GetCellContent(selectedPosition).FindName("cbProg") as ComboBox;
                if (cbProg != null)
                {
                    cbProg.ItemsSource = softwareList;
                }
                var cbAllProg = positionDataGrid.Columns[2].GetCellContent(selectedPosition).FindName("cbAllProg") as ComboBox;
                if (cbAllProg != null)
                {
                    cbAllProg.ItemsSource = new ObservableCollection<Software>(KonfigKc.Softwares.ToList());
                }
            }
        }
        private class PositionViewModel
        {
            public int PositionID { get; set; }
            public ObservableCollection<Software> Programs { get; set; }
            public ObservableCollection<Software> AllPrograms { get; set; }
        }
        private void PositionDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }
    }
}
