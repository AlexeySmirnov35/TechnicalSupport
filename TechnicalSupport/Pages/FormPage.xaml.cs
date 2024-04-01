using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;

namespace TechnicalSupport.Pages
{
    /// <summary>
    /// Логика взаимодействия для FormPage.xaml
    /// </summary>
    public partial class FormPage : Page
    {
        private Request _requests = new Request();
        ApplicationContext KonfigKc;

        public FormPage()
        {
            InitializeComponent();
            KonfigKc = new ApplicationContext();
            var context = KonfigKc;
            cbPosir.ItemsSource = KonfigKc.Positions.ToList();
            cbDepar.ItemsSource = KonfigKc.Departments.ToList();
            cbOtvest.ItemsSource = KonfigKc.Users.ToList();
            UpdatePo();
            var lastRequestId = context.Requests.Max(r => (int?)r.RequestID) ?? 0;
            if (lastRequestId != null)
            {
                int newRequestId = lastRequestId + 1;
                tbInc.Text = newRequestId.ToString();
            }
            else
            {
                MessageBox.Show("Возникла ошибка создание заявки");
            }
        }

        private void UpdatePo()
        {
            if (cbPosir.SelectedItem != null)
            {
                Position selectedPosition = cbPosir.SelectedItem as Position;

                if (selectedPosition != null)
                {
                    var softwareForPosition = selectedPosition.SoftwarePositions
                        .Select(sp => sp.Software.SoftwareName)
                        .ToList();

                    if (softwareForPosition.Any())
                    {
                        StringBuilder numberedSoftwareList = new StringBuilder();
                        for (int i = 0; i < softwareForPosition.Count; i++)
                        {
                            numberedSoftwareList.AppendLine($"{i + 1}. {softwareForPosition[i]}");
                        }

                        tbPO.Text = numberedSoftwareList.ToString();
                    }
                    else
                    {
                        tbPO.Text = "Нет данных для отображения.";
                    }
                }
            }
        }

        private void AddComboBox_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxPanel.Children.OfType<ComboBox>().Count() < 4)
            {
                ComboBox newComboBox = new ComboBox
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 398,
                    Height = 43,
                    FontSize = 28,
                    FontFamily = new FontFamily("Courier New"),
                    Margin = new Thickness(0, 10, 0, 0)
                };

                int comboBoxCount = comboBoxPanel.Children.OfType<ComboBox>().Count();
                string comboBoxName = "cbOtvest" + comboBoxCount.ToString();
                newComboBox.Name = comboBoxName;

                newComboBox.ItemsSource = KonfigKc.Users.ToList();
                newComboBox.ItemTemplate = this.Resources["FullNameTemplate"] as DataTemplate;

                comboBoxPanel.Children.Add(newComboBox);
            }
            else
            {
                MessageBox.Show("Нельзя добавить больше четырех исполнителей");
            }
        }







        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePo();
        }

        private void Create_Req_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем данные
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tbDesc.Text))
                errors.AppendLine("Напишите описание!");
            if (cbDepar.SelectedItem == null)
                errors.AppendLine("Укажите подразделения");
            if (cbPosir.SelectedItem == null)
                errors.AppendLine("Укажите должность!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Создаем запись для заявки
            Request request = new Request
            {
                DepartmentID = (cbDepar.SelectedItem as Department).DepartmentID,
                PositionID = (cbPosir.SelectedItem as Position).PositionID,
                StatusID = 1,
                Description = tbDesc.Text,
                RequestDateStart = DateTime.Now.ToString(),
                RequestDateFinish = null,
            };

            // Добавляем заявку в контекст данных
            KonfigKc.Requests.Add(request);

            // Создаем множество для отслеживания уже добавленных пользователей к заявке
            HashSet<int> addedUserIDs = new HashSet<int>();

            // Создаем записи для ответственных
            foreach (ComboBox comboBox in comboBoxPanel.Children.OfType<ComboBox>())
            {
                if (comboBox.SelectedItem != null)
                {
                    User user = comboBox.SelectedItem as User;
                    if (user != null && !addedUserIDs.Contains(user.UserID))
                    {
                        // Создаем запись для связи заявки с ответственным
                        RequestUser requestUser = new RequestUser
                        {
                            RequestID = request.RequestID,
                            UserID = user.UserID
                        };

                        // Добавляем запись в контекст данных
                        KonfigKc.RequestUsers.Add(requestUser);

                        // Добавляем идентификатор пользователя в множество добавленных пользователей
                        addedUserIDs.Add(user.UserID);
                    }
                }
            }

            try
            {
                // Сохраняем изменения
                KonfigKc.SaveChanges();
                MessageBox.Show("Успешно сохранено");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
