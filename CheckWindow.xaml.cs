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

        static void SqlInfoMessageEventHandler(object sender, SqlInfoMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        string connectionString = "Data Source= R54-630099-S;Initial Catalog=DB630099;Integrated Security= SSPI;";
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            string query = "DBCC CHECKDB ();";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.InfoMessage += SqlInfoMessageEventHandler;
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = 2147483647;
                int count = command.ExecuteNonQuery();
                Console.WriteLine("{0}", count);
                connection.Close();
                Console.ReadLine();
            }
        }
    }
}
