using System;
using System.Linq;
using System.Text;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    public partial class AddEditRoleWindow : Window
    {
        private ApplicationContext KonfigKc;
        private Position _role;

        public AddEditRoleWindow(Position role)
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            _role = role ?? new Position();
            DataContext = _role;

            LoadOperatingSystems();
            if (role != null)
            {
                tbPos.Text = role.PositionName;
                cbFile.SelectedItem = KonfigKc.OperatingSystems.FirstOrDefault(os => os.OperatingSystemsID == role.OperatingSystemsID);
            }
        }

        private void LoadOperatingSystems()
        {
            cbFile.ItemsSource = KonfigKc.OperatingSystems.ToList();
        }

        private void AddEditRole_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields(out StringBuilder errors))
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                if (_role.PositionID == 0)
                {
                    AddRole();
                }
                else
                {
                    UpdateRole();
                }

                MessageBox.Show("Операция прошла успешно");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении должности: {ex.Message}");
                Console.WriteLine($"Ошибка при сохранении должности: {ex}");
            }
        }

        private bool ValidateFields(out StringBuilder errors)
        {
            errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tbPos.Text))
            {
                errors.AppendLine("Введите корректное название должности.");
                return false;
            }

            if (_role.PositionID == 0 && KonfigKc.Positions.Any(sp => sp.PositionName == tbPos.Text))
            {
                errors.AppendLine("Такая запись существует.");
                return false;
            }

            return true;
        }

        private void AddRole()
        {
            var newPosition = new Position
            {
                PositionName = tbPos.Text,
                OperatingSystemsID = (cbFile.SelectedItem as DataBaseClasses.OperatingSystem)?.OperatingSystemsID ?? 1
            };

            KonfigKc.Positions.Add(newPosition);
            KonfigKc.SaveChanges();
            Console.WriteLine($"Добавлена новая должность: {newPosition.PositionName}");
        }

        private void UpdateRole()
        {
            _role.PositionName = tbPos.Text;
            _role.OperatingSystemsID = (cbFile.SelectedItem as DataBaseClasses.OperatingSystem)?.OperatingSystemsID ?? _role.OperatingSystemsID;
            KonfigKc.SaveChanges();
            Console.WriteLine($"Обновлена должность: {_role.PositionName}");
        }
    }
}
