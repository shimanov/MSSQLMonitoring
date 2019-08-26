using System;
using System.Data.SqlClient;
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

        //static void SqlInfoMessageEventHandler(object sender, SqlInfoMessageEventArgs e)
        //{
        //    //Console.WriteLine(e.Message);
        //    //resultTb.Text = 
        //}

        string connectionString = "Data Source= 10.0.75.1;Initial Catalog=DB633541;User id=sa;Password=qwep[]ghjB1";
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.Visibility = Visibility.Collapsed;
            StartBtn.Visibility = Visibility.Visible;

            string query = "DBCC CHECKDB ();";

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
