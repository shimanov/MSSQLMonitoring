using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace DatabaseMaintenance
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        readonly TempFileStorage.TempFileStorage fileStorage = new TempFileStorage.TempFileStorage();

        public LoginWindow()
        {
            InitializeComponent();
            fileStorage.DeleteFolder("Auth");
            fileStorage.DeleteFolder("CheckResult");

            fileStorage.CreateFolder();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectCmb.SelectedIndex == 0)
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

                        fileStorage.CreateFile("Auth", connectionString);
                        new MainWindow().Show();
                        Close();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            }
            if(selectCmb.SelectedIndex == 1)
            {
                string serverName = ServerNameTxb.Text;

                string connectionStringWin = "Data Source=" + serverName + ";Initial Catalog=master; Integrated security = SSPI;";
                using (SqlConnection connection = new SqlConnection(connectionStringWin))
                {
                    try
                    {
                        connection.Open();
                        fileStorage.CreateFile("Auth", connectionStringWin);
                        new MainWindow().Show();
                        Close();                        
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            }
        }

        private void SelectCmb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string domain = Environment.UserDomainName;
            if (selectCmb.SelectedIndex == 1)
            {
                UserTxb.IsEnabled = false;
                UserTxb.Text = domain + "/" + Environment.UserName.ToUpper();
                PasswordTxb.IsEnabled = false;
                PasswordTxb.Password = string.Empty;
            }
        }
    }
}
