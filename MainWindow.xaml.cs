﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseMaintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string server, string user, string password)
        {
            InitializeComponent();

            string queryString = "use master select (physical_memory_in_use_kb/1024)Phy_Memory_usedby_Sqlserver_MB from sys. dm_os_process_memory";
            string connectionString = "Data Source=" + server + ";Initial Catalog=master;User Id=" + user + ";Password =" + password;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        memoryCard.Content = "Объем использованной ОЗУ SQL: " + reader[0] + " MB";
                    }
                }
                connection.Close();
            }

            databaseData(connectionString);
            versionServer(connectionString);
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
            }
        }

        private void versionServer(string conn)
        {
            string query = "use master SELECT SERVERPROPERTY('ProductVersion'), SERVERPROPERTY('InstanceName'), SERVERPROPERTY('Edition');";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        versionCard.Content = "Версия MS SQL Server: " + reader[0] + '\n' + reader[1] + '\n' + reader[2];
                    }
                }
                connection.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
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
            //new CheckWindow(string server, string user, string password).Show();

            CheckWindow checkWindow = new CheckWindow();
            checkWindow.Show();
        }
    }
}
