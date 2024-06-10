﻿using System;
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
using TechnicalSupport.Pages;
using TechnicalSupport.DataBaseClasses;
using System.Runtime.Remoting.Contexts;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Interaction logic for MasterGlavWindow.xaml
    /// </summary>
    public partial class MasterGlavWindow : Window
    {
        private User _user=new User();
        ApplicationContext context;
        public MasterGlavWindow(User user)
        {
            InitializeComponent();
            context = new ApplicationContext();
            _user = user;
            DataContext = _user;
        }
        private void Btn_AddRequest(object sender, RoutedEventArgs e)
        {
         //   NavigationService.Navigate(new Uri("Pages/FormPage.xaml", UriKind.Relative));
        }

        private void Btn_Jornul_Click(object sender, RoutedEventArgs e)
        {
           // NavigationService.Navigate(new Uri("Pages/PageJornul.xaml", UriKind.Relative));
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // NavigationService.Navigate(new Uri("Pages/UserSoft.xaml", UriKind.Relative));
        }

        private void Perexod_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new DepartPage());
        }

        private void PerexodRole_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new FilePage());
        }

        private void Perexod1_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new FormPage());
        }

        private void Upravlenie_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new MasterAreaPage(_user));
        }

        private void ProgramnoeObespechenie_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new UserSoft());
        }

        private void OperatsionnyeSystemy_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new OperSystemPage());
        }

        private void OborudovanieUpravlenie_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new OfficeEquipPage());

        }

        private void CniiSetPodrazdelenie_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new DepartPage());
        }

        private void CniiSetDoljnosti_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new MasterAreaPage(_user));

        }

        private void CniiSetRoli_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new RolePage());
        }

        private void BazaZnaniy_Click(object sender, RoutedEventArgs e)
        {
            // AddTab("База Знаний", new Uri("EditPageInformation.xaml", UriKind.Relative));
            //  frmMain.Navigate(new EditPageInformation());
            frmMain.Navigate(new InfoPage());

        }
        private void Faily_Click(object sender, RoutedEventArgs e)
        {
            //  AddTab("Файлы", new Uri("FilePage.xaml", UriKind.Relative));
            frmMain.Navigate(new FilePage());
        }

        private void Zayavki_Click(object sender, RoutedEventArgs e)
        {
            // AddTab("База Знаний", new Uri("FormPage.xaml", UriKind.Relative));
            frmMain.Navigate(new FormPage());
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void OborudovanieUpravleniePosi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProgramnoeObespecheniePosit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
