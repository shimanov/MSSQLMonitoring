using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace DatabaseMaintenance
{
    /// <summary>
    /// Логика взаимодействия для CheckWindow.xaml
    /// </summary>
    public partial class CheckWindow : Window
    {
        public CheckWindow()
        {
            InitializeComponent();
        }

        readonly string connectionString = File.ReadAllText(Directory.GetCurrentDirectory() + "/DatabaseMaintenance");
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.Visibility = Visibility.Collapsed;
            StartBtn.Visibility = Visibility.Visible;

            string query = "DBCC CHECKDB();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                {
                    resultTxb.Content += "\n" + e.Message;
                };
                connection.FireInfoMessageEventOnUserErrors = true;
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = 2147483647;
                int count = command.ExecuteNonQuery();
                connection.Close();

            }
        }
    }
}
