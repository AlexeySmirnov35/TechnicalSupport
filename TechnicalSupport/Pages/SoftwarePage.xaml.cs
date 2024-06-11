using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.Pages
{
    public partial class SoftwarePage : Page
    {
        private ApplicationContext KonfigKc;

        public SoftwarePage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            LoadSoftwareTypes();
            listview.ItemsSource = KonfigKc.Softwares.ToList();
        }

        private void LoadSoftwareTypes()
        {
            var softwareTypes = KonfigKc.TypeSofwares.ToList();
            ComboBoxSoftwareType.ItemsSource = softwareTypes;
            ComboBoxSoftwareType.DisplayMemberPath = "TypeName"; 
        }

        private void Tbox_Search(object sender, TextChangedEventArgs e)
        {
            FilterSoftware();
        }

        private void ComboBoxSoftwareType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterSoftware();
        }

        private void FilterSoftware()
        {
            var searchText = TboxSerch.Text.ToLower();
            var selectedType = ComboBoxSoftwareType.SelectedItem as TypeSofware;

            var filteredList = KonfigKc.Softwares.Where(s =>
                s.SoftwareName.ToLower().Contains(searchText) &&
                (selectedType == null || s.TypeSofwareID == selectedType.TypeSofwareID)).ToList();

            listview.ItemsSource = filteredList;
        }

        private void SoftwareListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview.SelectedItem != null)
            {
                Software selectedSoftware = (Software)listview.SelectedItem;
                SoftInfoPage editSoftwarePage = new SoftInfoPage(selectedSoftware);
                NavigationService.Navigate(editSoftwarePage);
            }
        }

        private void Add_Soft_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SoftInfoPage(null));
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Btn_Del(object sender, RoutedEventArgs e)
        {
            var softToDelete = listview.SelectedItems.Cast<Software>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить эти {softToDelete.Count} элемента!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                foreach (var softw in softToDelete)
                {
                    if (!KonfigKc.SoftwarePositions.Any(item => item.SoftwareID == softw.SoftwareID))
                    {
                        KonfigKc.Softwares.Remove(softw);
                    }
                    else
                    {
                        MessageBox.Show($"Файл {softw.SoftwareName} используется в других таблицах и не может быть удален.");
                    }
                }

                KonfigKc.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                listview.ItemsSource = KonfigKc.Softwares.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
