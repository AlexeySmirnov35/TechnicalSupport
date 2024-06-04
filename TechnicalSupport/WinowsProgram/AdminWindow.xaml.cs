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
using TechnicalSupport.Pages;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            //FramMain.Navigate(new DepartPage());
        }

        private void DelDepar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Perexod_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new DepartPage());
        }

        private void PerexodRole_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new FilePage());
        }
    }
}
