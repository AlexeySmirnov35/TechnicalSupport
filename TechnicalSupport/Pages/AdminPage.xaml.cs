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
using TechnicalSupport;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        ApplicationContext KonfigKc;
       
        public AdminPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
        }

        private void Edit_bd_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new Uri("Pages/InfoPage.xaml", UriKind.Relative));
        }

        private void Role_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new SoftwarePage());
        }

        private void Depart_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new DepartPage());
        }

        private void Soft_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/SoftwarePage.xaml", UriKind.Relative));
        }

        private void GoNack_Click(object sender, RoutedEventArgs e)
        {
           
        }

       /* private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (var textBlock in FindVisualChildren<TextBlock>(frmMain))
            {
                textBlock.FontSize = CalculateFontSizeForTextBlock(textBlock);
            }
        }

         private void MainFrame_ContentRendered(object sender, EventArgs e)
         {
             foreach (var textBlock in FindVisualChildren<TextBlock>(frmMain))
             {
                 textBlock.FontSize = CalculateFontSizeForTextBlock(textBlock);
             }
         }
         private double CalculateFontSizeForTextBlock(TextBlock textBlock)
         {
             // Получение текущего DPI (dots per inch) экрана пользователя
             PresentationSource source = PresentationSource.FromVisual(textBlock);
             double dpiX = 96.0;
             double dpiY = 96.0;
             if (source != null)
             {
                 dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                 dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
             }

             // Здесь можно использовать текущий DPI и другие параметры для расчета размера шрифта
             // Например, можно адаптировать размер шрифта в зависимости от DPI и разрешения экрана

             double desiredFontSize = 16;

             // Пример логики, учитывающей DPI экрана
             double scaleFactor = Math.Max(dpiX, dpiY) / 96.0; // 96.0 - стандартный DPI
             desiredFontSize *= scaleFactor; // Масштабирование размера шрифта в зависимости от DPI

             return desiredFontSize;
         }

         private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
         {
             if (depObj != null)
             {
                 for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                 {
                     DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                     if (child != null && child is T)
                     {
                         yield return (T)child;
                     }

                     foreach (T childOfChild in FindVisualChildren<T>(child))
                     {
                         yield return childOfChild;
                     }
                 }
             }
         }
    }
}
