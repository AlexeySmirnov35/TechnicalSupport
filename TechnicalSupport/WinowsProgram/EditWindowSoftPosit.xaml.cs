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
    /// Interaction logic for EditWindowSoftPosit.xaml
    /// </summary>
    public partial class EditWindowSoftPosit : Window
    {
        private readonly ApplicationContext _context;
        private readonly SoftwarePosition _softwarePosition;

        public EditWindowSoftPosit(SoftwarePosition softwarePosition)
        {
            InitializeComponent();
            _context = new ApplicationContext();
            _softwarePosition = softwarePosition;
            chkBoxLin.IsChecked = (_softwarePosition.LicenseTreb == 1);
            DataContext = _softwarePosition;
            cbAllProg.ItemsSource=_context.Softwares.ToList();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            var prog = cbAllProg.SelectedItem as Software;
            int lin = chkBoxLin.IsChecked == true ? 1 : 0;

            StringBuilder errors = new StringBuilder();

            if (prog == null)
                errors.AppendLine("Выберите программу для замены");

            var isDuplicate = _context.SoftwarePositions
                .Any(sp => sp.PositionID == _softwarePosition.PositionID && sp.SoftwareID == prog.SoftwareID);

            if (isDuplicate)
                errors.AppendLine("Такая запись существует");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _softwarePosition.SoftwareID = prog?.SoftwareID ?? 0;
            _softwarePosition.LicenseTreb = lin;

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
