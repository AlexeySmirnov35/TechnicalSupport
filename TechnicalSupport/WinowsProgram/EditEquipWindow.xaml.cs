using System;
using System.Linq;
using System.Text;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для EditEquipWindow.xaml
    /// </summary>
    public partial class EditEquipWindow : Window
    {
        private readonly ApplicationContext _context;
        private readonly PositionOfficeEquip _originalSoftwarePosition;
        private readonly PositionOfficeEquip _editableSoftwarePosition;

        public EditEquipWindow(PositionOfficeEquip positionOfficeEquip, ApplicationContext context)
        {
            InitializeComponent();
            _context = context;
            _originalSoftwarePosition = positionOfficeEquip ?? new PositionOfficeEquip();
            _editableSoftwarePosition = new PositionOfficeEquip
            {
                PositionsID = _originalSoftwarePosition.PositionsID,
                OfficeEquipID = _originalSoftwarePosition.OfficeEquipID
            };
            DataContext = _originalSoftwarePosition;
            cbAllProg.ItemsSource = _context.OfficeEquipments.ToList();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            var prog = cbAllProg.SelectedItem as OfficeEquipment;

            StringBuilder errors = new StringBuilder();

            if (prog == null)
                errors.AppendLine("Выберите программу для замены");

            var isDuplicate = _context.PositionOfficeEquips
                .Any(sp => sp.PositionsID == _editableSoftwarePosition.PositionsID && sp.OfficeEquipID == prog.OfficeEquipmentID);

            if (isDuplicate)
                errors.AppendLine("Такая запись существует");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _editableSoftwarePosition.OfficeEquipID = prog?.OfficeEquipmentID ?? 0;

            try
            {
                if (_editableSoftwarePosition.PositionsID == 0)
                {
                    var newPositionOfficeEquip = new PositionOfficeEquip
                    {
                        PositionsID = _editableSoftwarePosition.PositionsID,
                        OfficeEquipID = _editableSoftwarePosition.OfficeEquipID
                    };

                    _context.PositionOfficeEquips.Add(newPositionOfficeEquip);
                }
                else
                {
                    _originalSoftwarePosition.OfficeEquipID = _editableSoftwarePosition.OfficeEquipID;
                }

                _context.SaveChanges();
                MessageBox.Show("Успешно сохранено");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}
