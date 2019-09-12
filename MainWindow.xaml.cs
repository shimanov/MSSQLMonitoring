using System.Data;
using System.Data.SqlClient;
using System.Windows;
using ScriptsLibrary;

namespace DatabaseMaintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly TempFileStorage.TempFileStorage fileStorage = new TempFileStorage.TempFileStorage();
        Script script = new Script();


        public MainWindow()
        {
            InitializeComponent();

            string connectionString = fileStorage.Read("Auth");

            databaseData(connectionString);
            versionServer(connectionString);
            memoryServer();
            sqlMemory();
        }

        private void memoryServer()
        {
            string res = script.ExecuteScript("UsingMemory.sql");

            memoryCard.Content = "Объем использованной ОЗУ SQL: " + res + " MB";
        }

        private void sqlMemory()
        {
            string res = script.ExecuteScript("TotalPhysMemory.sql");

            physMemoryCard.Content = "Общий объем ОЗУ: " + res + " MB";
        }

        private void databaseData(string conn)
        {
            string query = "use master select name as [Имя БД] ,state_desc as [Статус]  ,CAST(size / 128.0 as decimal(17,2)) as [Размер MB] from sys.master_files";
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }
                sqlConnection.Close();
            }
        }

        private void versionServer(string conn)
        {
            string query = "use master SELECT SERVERPROPERTY('ServerName') AS [Сервер], SERVERPROPERTY('ProductVersion') AS [Версия продукта], SERVERPROPERTY('Edition') AS [Редакция], SERVERPROPERTY('ProductLevel') AS [SP]; ";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    versionGrid.ItemsSource = dataTable.DefaultView;
                }
                connection.Close();
            }
        }

        private void EditConnBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
            memoryCard.Content = "Объем использованной ОЗУ SQL:";
        }

        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            new CheckWindow().Show();
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            fileStorage.DeleteFolder("Auth");
            fileStorage.DeleteFolder("CheckResult");
        }
    }
}
