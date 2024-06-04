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
using TechnicalSupport.DataBaseClasses;
using TechnicalSupport.WinowsProgram;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для DepartPageProb.xaml
    /// </summary>
    public partial class DepartPageProb : Page
    {
        ApplicationContext KonfigKcDB;

       
           
            public DepartPageProb()
        {
            InitializeComponent();
            KonfigKcDB = new ApplicationContext();
            listview.ItemsSource = KonfigKcDB.Departments.ToList();
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        }

        private void DelDepar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
