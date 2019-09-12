using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        readonly TempFileStorage.TempFileStorage fileStorage = new TempFileStorage.TempFileStorage();
        private readonly BackgroundWorker backgroundWorker;

        public CheckWindow()
        {
            InitializeComponent();

            DbList(fileStorage.Read("Auth"));

            backgroundWorker = ((BackgroundWorker)FindResource("backgroundWorker"));

            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
        }

        //Выполнение в фоне
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Check check = new Check();
            check.StartCheck();
        }

        private void BackgroundWorker_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            StartBtn.Visibility = Visibility.Hidden;
            ProgressBtn.Visibility = Visibility.Hidden;
            SaveBtn.Visibility = Visibility.Visible;

            resultLbl.Content = fileStorage.Read("CheckResult");

            MessageBox.Show("DONE!");
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.Visibility = Visibility.Hidden;
            ProgressBtn.Visibility = Visibility.Visible;

            Check check = new Check();

            backgroundWorker.RunWorkerAsync(check);
        }

        //Сохранение
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, resultLbl.Content.ToString());
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress progress = new Progress();
            ProgressBtn.Content = progress.Procent();
        }

        private void DbList(string conn)
        {
            string query = "use master select name from sys.databases";
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                dbCmb.ItemsSource = dataTable.DefaultView;
                dbCmb.DisplayMemberPath = "name";
                dbCmb.SelectedValuePath = "master";
                sqlConnection.Close();
            }
        }
    }

    class Check
    {
        readonly TempFileStorage.TempFileStorage fileStorage = new TempFileStorage.TempFileStorage();

        public void StartCheck()
        {
            string connectionString = fileStorage.Read("Auth");

            string query = "use DBCC CHECKDB() WITH NO_INFOMSGS, ALL_ERRORMSGS, DATA_PURITY;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                {
                    fileStorage.CreateFile("CheckResult", e.Message);
                };

                connection.Open();

                connection.FireInfoMessageEventOnUserErrors = true;
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = 2147483647;
                int count = command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    class Progress
    {
        readonly TempFileStorage.TempFileStorage fileStorage = new TempFileStorage.TempFileStorage();
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public int Procent()
        {
            string connectionString = fileStorage.Read("Auth");
            int perc = 0;

            string query = "USE master SELECT [percent_complete] FROM sys.dm_exec_requests WHERE [command] LIKE '%DBCC%'";

            if (backgroundWorker.WorkerReportsProgress)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            backgroundWorker.ReportProgress((int)reader.GetDouble(0));
                            perc = (int)reader.GetDouble(0);
                        }
                    }
                    connection.Close();
                }
            }
            return perc;
        }
    }
}
