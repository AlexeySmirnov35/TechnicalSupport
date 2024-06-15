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
    /// Логика взаимодействия для AdminPasswordDialog.xaml
    /// </summary>
    public partial class AdminPasswordDialog : Window
    {
        public string AdminPassword { get; private set; }

        public AdminPasswordDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPassword = pbPassword.Password;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void BtnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            if (tbPassword.Visibility == Visibility.Collapsed)
            {
                tbPassword.Visibility = Visibility.Visible;
                tbPassword.Text = pbPassword.Password;
                pbPassword.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-invisible-96.png"));
            }
            else
            {
                pbPassword.Visibility = Visibility.Visible;
                pbPassword.Password = tbPassword.Text;
                tbPassword.Visibility = Visibility.Collapsed;
                (button.Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/img/icons8-eye-96.png"));
            }
        }
    }
}
