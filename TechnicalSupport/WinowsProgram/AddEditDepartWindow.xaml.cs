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
        private readonly Department _originalDepartment;
        private readonly Department _editableDepartment;

        public AddEditDepartWindow(Department department, ApplicationContext context)
        {
            InitializeComponent();
            _context = context;
            _originalDepartment = department ?? new Department();
            _editableDepartment = new Department
            {
                DepartmentID = _originalDepartment.DepartmentID,
                DepartmentName = _originalDepartment.DepartmentName
            };
            DataContext = _editableDepartment;
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
                if (_editableDepartment.DepartmentID == 0)
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
                _context.Departments.Any(d => d.DepartmentName == departmentName && d.DepartmentID != _editableDepartment.DepartmentID));
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
            _originalDepartment.DepartmentName = _editableDepartment.DepartmentName;
            await _context.SaveChangesAsync();
            Console.WriteLine($"Обновлено подразделение: {_originalDepartment.DepartmentName}");
        }
    }
}
