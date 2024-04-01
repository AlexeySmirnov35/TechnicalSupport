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

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для RolePage.xaml
    /// </summary>
    public partial class RolePage : Page
    {
        ApplicationContext KonfigKc;

     
        public RolePage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            listview.ItemsSource=KonfigKc.Positions.ToList();
        }

        private void AddEditRole_Click(object sender, RoutedEventArgs e)
        {
            
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tbPos.Text))
            {
                errors.AppendLine("Введите корректное название должности");
            }
            else
            {
                var dbContext = KonfigKc;
                var isDuplicate = dbContext.Positions.Any(sp => sp.PositionName == tbPos.Text);

                if (isDuplicate)
                {
                    errors.AppendLine("Такая запись существует");
                }

                if (errors.Length == 0)
                {
                    Position newPosition = new Position{ PositionName = tbPos.Text };

                    if (listview.SelectedItem != null)
                    {
                        Position selectedPosition = (Position)listview.SelectedItem;
                        newPosition.PositionID = selectedPosition.PositionID;
                        UpdatePosition(newPosition);
                    }
                    else
                    {
                        dbContext.Positions.Add(newPosition);
                    }

                    dbContext.SaveChanges();
                    listview.SelectedItem = null;
                    listview.ItemsSource = dbContext.Positions.ToList();
                    tbPos.Clear();
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
            }
        }




        private void UpdatePosition(Position newPosition)
        {
            
            var existingPosition = KonfigKc.Positions.Find(newPosition.PositionID);
             if (existingPosition != null)
            {
                existingPosition.PositionName = newPosition.PositionName;
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DelRole_Click(object sender, RoutedEventArgs e)
        {
            var positionsToDelete = listview.SelectedItems.Cast<Position>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить эти {positionsToDelete.Count()} элемента!?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var dbContext = KonfigKc;

                foreach (var position in positionsToDelete)
                {
                    if (!IsPositionUsedInOtherTables(position))
                    {
                        dbContext.Positions.Remove(position);
                    }
                    else
                    {
                        MessageBox.Show($"Должность {position.PositionName} используется в других таблицах и не может быть удалена.");
                    }
                }

                dbContext.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                listview.ItemsSource = dbContext.Positions.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении должности: {ex.Message}");
            }
        }

        private bool IsPositionUsedInOtherTables(Position position)
        {
            var dbContext = KonfigKc;
            return dbContext.SoftwarePositions.Any(item => item.PositionID == position.PositionID)
                || dbContext.Requests.Any(item => item.PositionID == position.PositionID);
        }



    }
}

