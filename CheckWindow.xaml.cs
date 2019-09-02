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

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.Visibility = Visibility.Hidden;
            ProgressBtn.Visibility = Visibility.Visible;

            backgroundWorker.RunWorkerAsync();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, resultTxb.Content.ToString());
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string query = "DBCC CHECKDB() WITH NO_INFOMSGS, ALL_ERRORMSGS, DATA_PURITY;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                {
                    using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "/CheckDb.rpt"))
                    {
                        writer.Write(e.Message);
                    }
                };

                connection.Open();

                connection.FireInfoMessageEventOnUserErrors = true;
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = 2147483647;
                int count = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string query = "";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection)
                    {
                        CommandTimeout = 30
                    };
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string command = reader.GetString(0);
                                double percent = reader.GetDouble(2);
                                double time = reader.GetDouble(3);

                                //double s = Convert.ToDouble(result, CultureInfo.InvariantCulture);
                                //double s1 = Convert.ToDouble(result1, CultureInfo.InvariantCulture);

                                commandLbl.Content = command;
                                timeLbl.Content = percent;
                                ProgressBtn.Content = time;
                            }
                        }
                        reader.Close();
                    }
                    sqlConnection.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ProgressBtn.Visibility = Visibility.Hidden;
                SaveBtn.Visibility = Visibility.Visible;
                resultTxb.Content = File.ReadAllText(Directory.GetCurrentDirectory() + "/CheckDb.rpt");
            }
        }
    }
}
