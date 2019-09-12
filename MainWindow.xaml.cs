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

            databaseData();
            versionServer();
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

        private void databaseData()
        {
            dataGrid.ItemsSource = script.ExecuteScriptTable("DataBases.sql");
        }

        private void versionServer()
        {
            versionGrid.ItemsSource = script.ExecuteScriptTable("InfoMssql.sql");
        }

        private void EditConnBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
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
