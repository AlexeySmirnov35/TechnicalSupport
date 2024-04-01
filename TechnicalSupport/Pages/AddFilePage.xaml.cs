using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для AddFilePage.xaml
    /// </summary>
    public partial class AddFilePage : Page
    {
        public Department Department { get; set; }
        public AddFilePage()
        {
            
            InitializeComponent();
            frmMain.Navigate(new PersonalArea());
        }

        private void add(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelDepar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_GoBack(object sender, RoutedEventArgs e)
        {

        }
        /*   private void CreateReservation(int userId)
{
try
{
StringBuilder errors = new StringBuilder();

var hour = cbTime.SelectedItem;
var room = cbRoom.SelectedItem as Rooms;
var hourbron = cbHour.SelectedItem;
int selectedHours;

string selectedTimeString = cbTime.SelectedItem.ToString();
DateTime selectedTime = DateTime.ParseExact(hour.ToString(), "HH:mm", CultureInfo.InvariantCulture);
int selectedHour = selectedTime.Hour;

if (!int.TryParse(cbHour.SelectedItem?.ToString(), out selectedHours) || selectedHours <= 0)
{
errors.AppendLine("Выберите количество часов.");
}

if (room == null || hour == null || hourbron == null || string.IsNullOrWhiteSpace(tbNum.Text) ||
string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbSur.Text) || string.IsNullOrWhiteSpace(tbDop.Text))
{
errors.AppendLine("Выберите комнату, время и количество часов.");
}

if (!int.TryParse(tbNum.Text, out int numberOfPeople) || numberOfPeople <= 1)
{
errors.AppendLine("Количество людей должно быть больше 1.");
}

if (room != null && room.NumberPeopleMax < numberOfPeople)
{
errors.AppendLine("Количество людей не может превышать максимальное количество людей в комнате.");
}

if (selectedHours + selectedHour > 22)
{
errors.AppendLine("Выбранное количество часов не может превышать 22 часа.");
}

int selectedMinutes;
if (!int.TryParse(tbDop.Text, out selectedMinutes) || selectedMinutes < 0)
{
errors.AppendLine("Введите корректное значение для дополнительных минут.");
}

if (errors.Length > 0)
{
MessageBox.Show(errors.ToString());
return;
}

string fios = $"{tbSur.Text} {tbName.Text} {tbPat.Text}";
Reservations newd = new Reservations();
DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now.Date;
DateTime combinedDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedTime.Hour, selectedTime.Minute, 0);
newd.DateTimeReserv = combinedDateTime;

var endDate = newd.DateTimeReserv.Value.AddHours(selectedHours).AddMinutes(selectedMinutes);
var overlappingReservations = RestoranEntities.Reservations
.Where(r => r.RoomID == room.RoomID &&
((newd.DateTimeReserv >= r.DateTimeReserv && newd.DateTimeReserv < r.DateEndReserv) ||
(endDate > r.DateTimeReserv && endDate <= r.DateEndReserv)))
.ToList();

if (overlappingReservations.Any())
{
MessageBox.Show("Выбранное количество часов и минут не может быть забронировано.");
return;
}

Reservations reservation = new Reservations()
{
UserID = userId,
FIO = fios,
RoomID = room.RoomID,
NumberOfPeople = Convert.ToInt32(tbNum.Text),
Hours = selectedHours,
DopTimeMinut = selectedMinutes,
DateTimeReserv = newd.DateTimeReserv,
DateEndReserv = endDate
};

RestoranEntities.Reservations.Add(reservation);
RestoranEntities.SaveChanges();
MessageBox.Show("Бронирование успешно сохранено");
}
catch (Exception ex)
{
MessageBox.Show($"Произошла ошибка при сохранении бронирования: {ex.Message}");
}

DatePicker_SelectedChanged(datePicker, null);
}*/
    }
}
