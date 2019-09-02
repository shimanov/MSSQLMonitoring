using Microsoft.Win32;
using System;
using System.ComponentModel;
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
        //readonly string connectionString = File.ReadAllText(Directory.GetCurrentDirectory() + "/DatabaseMaintenance");
        readonly string connectionString = "Data Source=10.0.75.1;Initial Catalog =DB633541; User id=sa; password=qwep[]ghjB1";

        private BackgroundWorker backgroundWorker;

        public CheckWindow()
        {
            InitializeComponent();
            backgroundWorker = (BackgroundWorker)FindResource("backgroundWorker");
        }

        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.Visibility = Visibility.Hidden;
            ProgressBtn.Visibility = Visibility.Visible;


            Task task = Task.Run(() => CheckDb());
            await task;

        }


        private async Task CheckDb()
        {
            string query = "DBCC CHECKDB() WITH NO_INFOMSGS, ALL_ERRORMSGS, DATA_PURITY;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                {
                    resultTxb.Content += "\n" + e.Message;
                };

                await connection.OpenAsync();

                connection.FireInfoMessageEventOnUserErrors = true;
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = 2147483647;
                int count = command.ExecuteNonQuery();
                await connection.CloseAsync();

                ProgressBtn.Visibility = Visibility.Hidden;
                SaveBtn.Visibility = Visibility.Visible;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, resultTxb.Content.ToString());
            }
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }
    }
}
