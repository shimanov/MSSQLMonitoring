using System;
using System.Data.SqlClient;
using System.Windows;

namespace DatabaseMaintenance
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            string serverName = ServerNameTxb.Text;
            string user = UserTxb.Text;
            string password = PasswordTxb.Password;

            string connectionString = "Data Source=" + serverName + ";Initial Catalog=master;User Id=" + user + ";Password =" + password;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    new MainWindow(serverName, user, password).Show();

                    Close();
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }
    }
}
