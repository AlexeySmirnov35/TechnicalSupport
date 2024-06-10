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
using System.Windows.Shapes;
using TechnicalSupport.DataBaseClasses;

namespace TechnicalSupport.WinowsProgram
{
    /// <summary>
    /// Логика взаимодействия для AddCommitWindow.xaml
    /// </summary>
    public partial class AddCommitWindow : Window
    {
        ApplicationContext context;
        private Request _request = new Request();
        private User _user = new User();

        public AddCommitWindow(Request request, User user)
        {
            InitializeComponent();
            context = new ApplicationContext();
            _request = request;
            _user = user;
        }

        private void AddEditDepar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbDep.Text))
            {
                MessageBox.Show("Error");
                return;
            }

            CommitMessage commitMessage = new CommitMessage
            {
                UserID = _user.UserID,
                CommitTextMessage = tbDep.Text,
                RequestID = _request.RequestID
            };

            if (_user.RoleID == 1)
            {
                _request.StatusID = 5;
            }
            else
            {
                _request.StatusID = 3;

            }

            _request.RequestDateFinish = DateTime.Now.ToString();
            context.CommitMessages.Add(commitMessage);
            context.SaveChanges();
            MessageBox.Show("Save");
            this.DialogResult = true;
            // Закройте окно после сохранения
            this.Close();
        }
    }
}
