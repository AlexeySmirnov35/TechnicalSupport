using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для EditPageJornul.xaml
    /// </summary>
    public partial class EditPageJornul : Page
    {
        private Request _requests = new Request();
        ApplicationContext KonfigKc;


        public EditPageJornul(Request selectedRequests)
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            if (selectedRequests != null)
            {
                _requests = selectedRequests;
            }
            DataContext = _requests;
            UpdatePo();
        }
        private void UpdatePo()
        {
            try
            {
                if (_requests != null && _requests.Position != null)
                {
                    Position selectedPosition = _requests.Position;

                    if (selectedPosition != null && selectedPosition.SoftwarePositions!= null)
                    {
                        var softwareForPosition = selectedPosition.SoftwarePositions
                            .Select(sp => sp.Software.SoftwareName)
                            .ToList();

                        if (softwareForPosition.Any())
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < softwareForPosition.Count; i++)
                            {
                                sb.AppendLine($"{i + 1}. {softwareForPosition[i]}");
                            }
                            tbPO.Text = sb.ToString();
                        }
                        else
                        {
                            tbPO.Text = "Нет данных для отображения.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}");
            }
        }
           

        private void Create_Pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); 
        }
    }
}
