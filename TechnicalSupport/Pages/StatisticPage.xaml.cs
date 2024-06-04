using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.Pages
{
    public partial class StatisticPage : Page
    {
        private readonly ApplicationContext _context;

        public StatisticPage()
        {
            InitializeComponent();
            _context = new ApplicationContext();
            cbOtvest.ItemsSource = _context.Users.Where(x=>x.RoleID!=1&&x.RoleID!=6).ToList();
        }

        private void GetStatistics_Click(object sender, RoutedEventArgs e)
        {
            if (cbOtvest.SelectedItem == null)
            {
                MessageBox.Show("Invalid User ID");
                return;
            }

            int userId = (cbOtvest.SelectedItem as User).UserID;
            var statsService = new StatisticsService(_context);
            string period = ((ComboBoxItem)cbPeriod.SelectedItem).Tag.ToString();

            double avgCompletionTime = statsService.GetAverageCompletionTime(userId, period);
            int totalCompletedRequests = statsService.GetTotalCompletedRequests(userId, period);
            int totalPendingRequests = statsService.GetTotalPendingRequests(userId, period);
            int totalRequests = totalCompletedRequests + totalPendingRequests;
            double completedPercentage = totalRequests > 0 ? (double)totalCompletedRequests / totalRequests * 100 : 0;
            double pendingPercentage = totalRequests > 0 ? (double)totalPendingRequests / totalRequests * 100 : 0;

            AverageCompletionTimeTextBlock.Text = $"Среднее время выполнения заявок: {avgCompletionTime} часов";
            TotalCompletedRequestsTextBlock.Text = $"Завершенные заявки: {totalCompletedRequests} ({completedPercentage:0.##}%)";
            TotalPendingRequestsTextBlock.Text = $"НЕ завершенные заявки: {totalPendingRequests} ({pendingPercentage:0.##}%)";
        }
    }

    public class StatisticsService
    {
        private readonly ApplicationContext _context;

        public StatisticsService(ApplicationContext context)
        {
            _context = context;
        }

        public double GetAverageCompletionTime(int userId, string period)
        {
            var requests = GetRequestsByPeriod(userId, period)
                .Where(r => r.StatusID == 3) // assuming StatusID 3 means 'Completed'
                .ToList()
                .Where(r => DateTime.TryParse(r.RequestDateStart, out _) && DateTime.TryParse(r.RequestDateFinish, out _))
                .Select(r => new
                {
                    StartDate = DateTime.Parse(r.RequestDateStart),
                    FinishDate = DateTime.Parse(r.RequestDateFinish)
                });

            if (!requests.Any())
                return 0;

            return requests.Average(r => (r.FinishDate - r.StartDate).TotalHours);
        }

        public int GetTotalCompletedRequests(int userId, string period)
        {
            return GetRequestsByPeriod(userId, period)
                .Where(r => r.StatusID == 3) // assuming StatusID 3 means 'Completed'
                .Count();
        }

        public int GetTotalPendingRequests(int userId, string period)
        {
            return GetRequestsByPeriod(userId, period)
                .Where(r => r.StatusID < 3) // assuming StatusID < 3 means 'Pending'
                .Count();
        }

        private IQueryable<Request> GetRequestsByPeriod(int userId, string period)
        {
            DateTime now = DateTime.Now;
            var requests = _context.Requests.Where(r => r.UserID == userId).AsEnumerable();

            switch (period)
            {
                case "year":
                    DateTime yearStart = new DateTime(now.Year, 1, 1);
                    requests = requests.Where(r => DateTime.TryParse(r.RequestDateStart, out var dateStart) && dateStart >= yearStart);
                    break;
                case "quarter":
                    int currentQuarter = (now.Month - 1) / 3 + 1;
                    DateTime quarterStart = new DateTime(now.Year, (currentQuarter - 1) * 3 + 1, 1);
                    requests = requests.Where(r => DateTime.TryParse(r.RequestDateStart, out var dateStart) && dateStart >= quarterStart);
                    break;
                case "month":
                    DateTime monthStart = new DateTime(now.Year, now.Month, 1);
                    requests = requests.Where(r => DateTime.TryParse(r.RequestDateStart, out var dateStart) && dateStart >= monthStart);
                    break;
                case "all":
                default:
                    break;
            }

            return requests.AsQueryable();
        }
    }
}
