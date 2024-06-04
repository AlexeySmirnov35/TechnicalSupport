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
    /// Логика взаимодействия для EditEquipWindow.xaml
    /// </summary>
    public partial class EditEquipWindow : Window
    {
        private readonly ApplicationContext _context;
        private readonly PositionOfficeEquip _softwarePosition;
        public EditEquipWindow(PositionOfficeEquip positionOfficeEquip)
        {
            InitializeComponent();
            _context = new ApplicationContext();
            _softwarePosition = positionOfficeEquip;
            DataContext = _softwarePosition;
            cbAllProg.ItemsSource = _context.OfficeEquipments.ToList();
        }

     

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            var prog = cbAllProg.SelectedItem as OfficeEquipment;

            StringBuilder errors = new StringBuilder();

            if (prog == null)
                errors.AppendLine("Выберите программу для замены");

            var isDuplicate = _context.PositionOfficeEquips
                .Any(sp => sp.PositionsID == _softwarePosition.PositionsID && sp.OfficeEquipID == prog.OfficeEquipmentID);

            if (isDuplicate)
                errors.AppendLine("Такая запись существует");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _softwarePosition.OfficeEquipID = prog?.OfficeEquipmentID ?? 0;

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Успешно сохранено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

