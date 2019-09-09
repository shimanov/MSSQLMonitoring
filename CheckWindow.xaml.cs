using Microsoft.Win32;
using System.ComponentModel;
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
        readonly TempFileStorage fileStorage = new TempFileStorage();
        private readonly BackgroundWorker backgroundWorker;

        public CheckWindow()
        {
            InitializeComponent();

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
    }

    class Check
    {
        readonly TempFileStorage fileStorage = new TempFileStorage();

        public void StartCheck()
        {
            string connectionString = fileStorage.Read("Auth");

            string query = "use DB633541 DBCC CHECKDB() WITH NO_INFOMSGS, ALL_ERRORMSGS, DATA_PURITY;";

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
}
