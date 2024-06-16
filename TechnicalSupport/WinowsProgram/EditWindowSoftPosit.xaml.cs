using System;
using System.Linq;
using System.Text;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Interaction logic for EditWindowSoftPosit.xaml
    /// </summary>
    public partial class EditWindowSoftPosit : Window
    {
        private readonly ApplicationContext _context;
        private readonly SoftwarePosition _originalSoftwarePosition;
        private readonly SoftwarePosition _editableSoftwarePosition;

        public EditWindowSoftPosit(SoftwarePosition softwarePosition, ApplicationContext context)
        {
            InitializeComponent();
            _context = context;
            _originalSoftwarePosition = softwarePosition ?? new SoftwarePosition();
            _editableSoftwarePosition = new SoftwarePosition
            {
                PositionID = _originalSoftwarePosition.PositionID,
                SoftwareID = _originalSoftwarePosition.SoftwareID,
                LicenseTreb = _originalSoftwarePosition.LicenseTreb
            };
            DataContext = _editableSoftwarePosition; // Corrected DataContext to _editableSoftwarePosition

            if (_originalSoftwarePosition.Software != null)
            {
                tbVProg.Text = _originalSoftwarePosition.Software.SoftwareName;
            }

            tbPosi.Text = _originalSoftwarePosition.Position?.PositionName;

            chkBoxLin.IsChecked = (_editableSoftwarePosition.LicenseTreb == 1);
            cbAllProg.ItemsSource = _context.Softwares.ToList();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            var prog = cbAllProg.SelectedItem as Software;
            int lin = chkBoxLin.IsChecked == true ? 1 : 0;
            StringBuilder errors = new StringBuilder();

            if (prog == null)
            {
                errors.AppendLine("Выберите программу для замены");
            }
            else
            {
                // Retrieve all SoftwarePositions and perform in-memory check for duplicates
                var existingSoftwarePositions = _context.SoftwarePositions.ToList();
                var isDuplicate = existingSoftwarePositions
                    .Any(sp => sp.PositionID == _editableSoftwarePosition.PositionID
                            && sp.SoftwareID == prog.SoftwareID
                            && sp.LicenseTreb == lin);

                if (isDuplicate)
                {
                    errors.AppendLine("Такая запись существует");
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _editableSoftwarePosition.SoftwareID = prog?.SoftwareID ?? 0;
            _editableSoftwarePosition.LicenseTreb = lin;

            try
            {
                if (_originalSoftwarePosition.PositionID == 0)
                {
                    var newSoftwarePosition = new SoftwarePosition
                    {
                        PositionID = _editableSoftwarePosition.PositionID,
                        SoftwareID = _editableSoftwarePosition.SoftwareID,
                        LicenseTreb = _editableSoftwarePosition.LicenseTreb
                    };

                    _context.SoftwarePositions.Add(newSoftwarePosition);
                }
                else
                {
                    _originalSoftwarePosition.SoftwareID = _editableSoftwarePosition.SoftwareID;
                    _originalSoftwarePosition.LicenseTreb = _editableSoftwarePosition.LicenseTreb;
                    _context.Entry(_originalSoftwarePosition).State = System.Data.Entity.EntityState.Modified; // Ensure original position is updated
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
