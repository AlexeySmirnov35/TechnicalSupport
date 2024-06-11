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
    /// Логика взаимодействия для AddEditSotWindow.xaml
    /// </summary>
    public partial class AddEditSotWindow : Window
    {
        private readonly ApplicationContext _context;
        private readonly Department _department;
        public AddEditSotWindow(Department department)
        {
            InitializeComponent();
            _context = new ApplicationContext();
            _department = department;
            DataContext = _department;
        }
      

        private  void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            string departmentName = tbDep.Text;

            if (string.IsNullOrEmpty(departmentName))
            {
                MessageBox.Show("Название подразделения не должно быть пустым.");
                return;
            }

           /* if (await DepartmentExists(departmentName))
            {
                MessageBox.Show("Такое подразделение уже есть!");
                return;
            }*/

            try
            {

                _department.DepartmentName = departmentName;
               _context.SaveChanges();


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

       /* private async Task<bool> DepartmentExists(string departmentName)
        {
            return await Task.Run(() =>
                _context.Departments.Any(d => d.DepartmentName == departmentName && d.DepartmentID != _department.DepartmentID));
        }*/

      /*  private async Task AddDepartment(string departmentName)
        {
            var newDepartment = new Department { DepartmentName = departmentName };
            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Добавлено новое подразделение: {newDepartment.DepartmentName}");
        }*/

       /* private async Task UpdateDepartment(string departmentName)
        {
            _department.DepartmentName = departmentName;
            await _context.SaveChangesAsync();
            Console.WriteLine($"Обновлено подразделение: {_department.DepartmentName}");
        }*/
    }
}
    
