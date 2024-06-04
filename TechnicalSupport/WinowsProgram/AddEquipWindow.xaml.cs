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
using System.Windows.Shapes;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AddEquipWindow.xaml
    /// </summary>
    public partial class AddEquipWindow : Window
    {
        private SoftwarePosition _software = new SoftwarePosition();
        private readonly ApplicationContext _dbContext;
        public AddEquipWindow()
        {

            InitializeComponent();
            _dbContext = new ApplicationContext();
            cbSoft.ItemsSource = _dbContext.OfficeEquipments.ToList();
            cbPosir.ItemsSource = _dbContext.Positions.ToList();
        }

        

     

        private void Btn_Create(object sender, RoutedEventArgs e)
        {
            var selectedSoftware = cbSoft.SelectedItem as OfficeEquipment;
            var selectedPosition = cbPosir.SelectedItem as Position;
      

            if (selectedSoftware == null || selectedPosition == null)
            {
                MessageBox.Show("Выберите программу и должность");
                return;
            }

            try
            {
                var newSoftwarePosition = new PositionOfficeEquip
                {
                    OfficeEquipID = selectedSoftware.OfficeEquipmentID,
                    PositionsID = selectedPosition.PositionID,
                    
                };

                _dbContext.PositionOfficeEquips.Add(newSoftwarePosition);
                _dbContext.SaveChanges();

                MessageBox.Show("Успешно сохранено");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _dbContext.Dispose();
        }
    }
}
