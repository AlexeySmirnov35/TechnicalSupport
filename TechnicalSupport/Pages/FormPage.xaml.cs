using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.Pages
{
    public partial class FormPage : Page
    {
        private Request _requests = new Request();
        private ApplicationContext KonfigKc;

        public FormPage()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            KonfigKc = new ApplicationContext();
            cbPosir.ItemsSource = KonfigKc.Positions.ToList();
            cbDepar.ItemsSource = KonfigKc.Departments.ToList();
            UpdatePo();

            var lastRequestId = KonfigKc.Requests.Max(r => (int?)r.RequestID) ?? 0;
            tbInc.Text = (lastRequestId + 1).ToString();
        }

        private void UpdatePo()
        {
            StringBuilder sb = new StringBuilder();

            if (cbPosir.SelectedItem is Position selectedPosition)
            {
                // Get and format the software list
                var softwareForPosition = selectedPosition.SoftwarePositions
                    .Select(sp => sp.Software.SoftwareName)
                    .ToList();

                sb.AppendLine("ПО:");
                if (softwareForPosition.Any())
                {
                    sb.AppendLine(string.Join(Environment.NewLine, softwareForPosition.Select((software, index) => $"{index + 1}. {software}")));
                }
                else
                {
                    sb.AppendLine("Нет данных для отображения.");
                }

                // Get and format the operating systems list
                var osForPosition = KonfigKc.OperatingSystems
                    .Where(os => os.Positions.Any(p => p.PositionID == selectedPosition.PositionID))
                    .Select(os => os.NameOperatingSystem)
                    .ToList();

                sb.AppendLine();
                sb.AppendLine("ОС:");
                if (osForPosition.Any())
                {
                    sb.AppendLine(string.Join(Environment.NewLine, osForPosition.Select((os, index) => $"{index + 1}. {os}")));
                }
                else
                {
                    sb.AppendLine("Нет данных для отображения.");
                }

                // Get and format the office equipment list
                var officeEquipmentForPosition = selectedPosition.PositionOfficeEquips
                    .Select(pe => pe.OfficeEquipment.NameOfficeEquipment)
                    .ToList();

                sb.AppendLine();
                sb.AppendLine("ОргТехника:");
                if (officeEquipmentForPosition.Any())
                {
                    sb.AppendLine(string.Join(Environment.NewLine, officeEquipmentForPosition.Select((oe, index) => $"{index + 1}. {oe}")));
                }
                else
                {
                    sb.AppendLine("Нет данных для отображения.");
                }
            }
            else
            {
                sb.AppendLine("Нет данных для отображения.");
            }

            tbPO.Text = sb.ToString();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePo();
        }

        private void Create_Req_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateFields())
                    return;

                var exsUser = KonfigKc.Clients.FirstOrDefault(x => x.NumberPhone == tbNumber.Text && x.Firstname == tbSurname.Text && x.Surname == tbName.Text && x.Patranomic == tbPAt.Text);
                if (exsUser == null)
                {
                    if (!ValidateClientData(out var client))
                        return;

                    KonfigKc.Clients.Add(client);
                    KonfigKc.SaveChanges();
                    CreateRequest(client.ClientID);
                }
                else
                {
                    CreateRequest(exsUser.ClientID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private bool ValidateFields()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tbNumber.Text))
                errors.AppendLine("Поле 'Номер' не должно быть пустым.");
            else if (!int.TryParse(tbNumber.Text, out _) || tbNumber.Text.Length < 3)
                errors.AppendLine("Поле 'Номер' должно быть числом и содержать более трех символов.");

            if (string.IsNullOrWhiteSpace(tbSurname.Text))
                errors.AppendLine("Поле 'Фамилия' не должно быть пустым.");

            if (string.IsNullOrWhiteSpace(tbName.Text))
                errors.AppendLine("Поле 'Имя' не должно быть пустым.");

            if (string.IsNullOrWhiteSpace(tbCab.Text))
                errors.AppendLine("Поле 'Кабинет' не должно быть пустым.");
            else if (!int.TryParse(tbCab.Text, out _) || tbCab.Text.Length < 2)
                errors.AppendLine("Поле 'Кабинет' должно быть числом и содержать более двух символов.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return false;
            }

            return true;
        }

        private bool ValidateClientData(out Client client)
        {
            client = null;

            if (cbDepar.SelectedItem == null)
            {
                MessageBox.Show("Выберите подразделение");
                return false;
            }
            if (cbPosir.SelectedItem == null)
            {
                MessageBox.Show("Выберите должность");
                return false;
            }

            client = new Client
            {
                NumberPhone = tbNumber.Text,
                Firstname = tbSurname.Text,
                Surname = tbName.Text,
                Patranomic = tbPAt.Text,
                Cabinet = tbCab.Text,
                DepartamentID = (cbDepar.SelectedItem as Department).DepartmentID,
                PositionID = (cbPosir.SelectedItem as Position).PositionID,
            };
            return true;
        }

        private void CreateRequest(int clientId)
        {
            try
            {
                DateTime dataLine = DateTime.Now.AddDays(3);
                StringBuilder errors = new StringBuilder();

                if (cbDepar.SelectedItem == null)
                    errors.AppendLine("Номер должен содержать только цифры!!");
                if (cbPosir.SelectedItem == null)
                    errors.AppendLine("Укажите должность!");

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }

                CreateRequestIfChecked(checkBox1, tbDesc1.Text, 2, clientId, dataLine);
                CreateRequestIfChecked(checkBox2, tbDesc2.Text, 3, clientId, dataLine);
                CreateRequestIfChecked(checkBox3, tbDesc3.Text, 4, clientId, dataLine);
                CreateRequestIfChecked(checkBox4, tbDesc4.Text, 5, clientId, dataLine);

                MessageBox.Show("Успешно сохранено");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void CreateRequestIfChecked(CheckBox checkBox, string description, int userId, int clientId, DateTime deadline)
        {
            if (checkBox.IsChecked == true)
            {
                var request = new Request
                {
                    StatusID = 4,
                    Description = description,
                    RequestDateStart = DateTime.Now.ToString(),
                    RequestDeadline = deadline.ToString(),
                    RequestDateFinish = null,
                    UserID = userId,
                    ClientID = clientId
                };

                KonfigKc.Requests.Add(request);
                KonfigKc.SaveChanges();
            }
        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

        }
    }
}
