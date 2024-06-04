using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    public partial class AddEditDepartWindow : Window
    {
        private readonly ApplicationContext _context;
        private readonly Department _department;

        public AddEditDepartWindow(Department department)
        {
            InitializeComponent();
            _context = new ApplicationContext();
            _department = department ?? new Department();
            DataContext = _department;
        }

        private async void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            string departmentName = tbDep.Text;

            if (string.IsNullOrEmpty(departmentName))
            {
                MessageBox.Show("Название подразделения не должно быть пустым.");
                return;
            }

            if (await DepartmentExists(departmentName))
            {
                MessageBox.Show("Такое подразделение уже есть!");
                return;
            }

            try
            {
                if (_department.DepartmentID == 0)
                {
                    await AddDepartment(departmentName);
                }
                else
                {
                    await UpdateDepartment(departmentName);
                }

                MessageBox.Show("Операция прошла успешно");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении подразделения: {ex.Message}");
                Console.WriteLine($"Ошибка при сохранении подразделения: {ex}");
            }
        }

        private async Task<bool> DepartmentExists(string departmentName)
        {
            return await Task.Run(() =>
                _context.Departments.Any(d => d.DepartmentName == departmentName && d.DepartmentID != _department.DepartmentID));
        }

        private async Task AddDepartment(string departmentName)
        {
            var newDepartment = new Department { DepartmentName = departmentName };
            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Добавлено новое подразделение: {newDepartment.DepartmentName}");
        }

        private async Task UpdateDepartment(string departmentName)
        {
            _department.DepartmentName = departmentName;
            await _context.SaveChangesAsync();
            Console.WriteLine($"Обновлено подразделение: {_department.DepartmentName}");
        }
    }
}
