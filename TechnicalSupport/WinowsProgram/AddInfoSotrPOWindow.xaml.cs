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
using TechnicalSupport.Pages;
using TechnicalSupport.DataBaseClasses;
namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AddInfoSotrPOWindow.xaml
    /// </summary>
    public partial class AddInfoSotrPOWindow : Window
    {
        private SoftwarePosition _software = new SoftwarePosition();
        private readonly ApplicationContext _dbContext;

        public AddInfoSotrPOWindow()
        {
            InitializeComponent();
            _dbContext = new ApplicationContext();
            cbSoft.ItemsSource = _dbContext.Softwares.ToList();
            cbPosir.ItemsSource = _dbContext.Positions.ToList();
        }

        private void Btn_Create(object sender, RoutedEventArgs e)
        {
            var selectedSoftware = cbSoft.SelectedItem as Software;
            var selectedPosition = cbPosir.SelectedItem as Position;
            var licenseRequired = chekBoxLin.IsChecked ?? false ? 1 : 0;

            if (selectedSoftware == null || selectedPosition == null)
            {
                MessageBox.Show("Выберите программу и должность");
                return;
            }

            try
            {
                var newSoftwarePosition = new SoftwarePosition
                {
                    SoftwareID = selectedSoftware.SoftwareID,
                    PositionID = selectedPosition.PositionID,
                    LicenseTreb = licenseRequired
                };

                _dbContext.SoftwarePositions.Add(newSoftwarePosition);
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