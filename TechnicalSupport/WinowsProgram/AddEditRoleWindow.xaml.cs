using System;
using System.Linq;
using System.Text;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    public partial class AddEditRoleWindow : Window
    {
        private readonly ApplicationContext _konfigKc;
        private readonly Position _originalRole;
        private readonly Position _editableRole;

        public AddEditRoleWindow(Position role, ApplicationContext konfigKc)
        {
            InitializeComponent();
            _konfigKc = konfigKc;
            _originalRole = role ?? new Position();
            _editableRole = new Position
            {
                PositionID = _originalRole.PositionID,
                PositionName = _originalRole.PositionName,
                OperatingSystemsID = _originalRole.OperatingSystemsID
            };
            DataContext = _editableRole;

            LoadOperatingSystems();
            if (role != null)
            {
                tbPos.Text = _editableRole.PositionName;
                cbFile.SelectedItem = _konfigKc.OperatingSystems.FirstOrDefault(os => os.OperatingSystemsID == _editableRole.OperatingSystemsID);
            }
        }

        private void LoadOperatingSystems()
        {
            cbFile.ItemsSource = _konfigKc.OperatingSystems.ToList();
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
                if (_editableRole.PositionID == 0)
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

            if (_editableRole.PositionID == 0 && _konfigKc.Positions.Any(sp => sp.PositionName == tbPos.Text))
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

            _konfigKc.Positions.Add(newPosition);
            _konfigKc.SaveChanges();
            Console.WriteLine($"Добавлена новая должность: {newPosition.PositionName}");
        }

        private void UpdateRole()
        {
            _originalRole.PositionName = _editableRole.PositionName;
            _originalRole.OperatingSystemsID = (cbFile.SelectedItem as DataBaseClasses.OperatingSystem)?.OperatingSystemsID ?? _originalRole.OperatingSystemsID;
            _konfigKc.SaveChanges();
            Console.WriteLine($"Обновлена должность: {_originalRole.PositionName}");
        }
    }
}
