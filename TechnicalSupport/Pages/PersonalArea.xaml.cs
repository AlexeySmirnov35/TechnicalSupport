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

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для PersonalArea.xaml
    /// </summary>
    public partial class PersonalArea : Page
    {
        public PersonalArea()
        {
            InitializeComponent();
            //frmMain.Navigate(new SoftwarePage());
        }

        private void add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddFilePage());
        }
    }
}
