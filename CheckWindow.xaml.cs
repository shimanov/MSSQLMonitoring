using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        //readonly string connectionString = File.ReadAllText(Directory.GetCurrentDirectory() + "/DatabaseMaintenance");
        readonly string connectionString = "Data Source=R54-630099-S;Initial Catalog =DB630099; Integrated Security = SSPI;";

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

        //private async Task CheckDb()
        //{
        //    string query = "DBCC CHECKDB();";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();

        //        connection.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
        //        {
        //            resultTxb.Content += "\n" + e.Message;
        //        };
        //        connection.FireInfoMessageEventOnUserErrors = true;
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.CommandTimeout = 2147483647;
        //        int count = command.ExecuteNonQuery();
        //        connection.Close();
        //    }
        //}
    }
}
