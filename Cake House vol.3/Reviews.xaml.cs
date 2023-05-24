using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Cake_House_vol._3
{
    /// <summary>
    /// Логика взаимодействия для Reviews.xaml
    /// </summary>
    public partial class Reviews : Window
    {
        string user = "";
        public Reviews(string username)
        {
            InitializeComponent();
            filling();
            user = username;
        }
        public class RewClass
        {
            public string name1 { get; set; }
            public string rewiew1 { get; set; }
        }
        public ObservableCollection<RewClass> Rews { get; set; }
        private async void filling()
        {
            Rews = new ObservableCollection<RewClass>();
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            string rews = "SELECT * FROM reviews";
            string users = "SELECT * FROM customers";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(rews, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object name = reader.GetValue(1);
                        object rewiew = reader.GetValue(2);
                        using (SqlConnection connection2 = new SqlConnection(connectionString))
                        {
                            await connection2.OpenAsync();

                            SqlCommand command2 = new SqlCommand(users, connection2);
                            SqlDataReader reader2 = await command2.ExecuteReaderAsync();

                            if (reader2.HasRows)
                            {
                                while (await reader2.ReadAsync())
                                {
                                    object userid = reader2.GetValue(0);
                                    object username = reader2.GetValue(1);
                                    object Byuer = reader2.GetValue(3);

                                    if(username.ToString() == user)
                                    {
                                        if (Convert.ToInt32(Byuer) == 1)
                                            leave.IsEnabled = true;
                                    }
                                    if(Convert.ToInt32(userid) == Convert.ToInt32(name))
                                    {
                                        Rews.Add(new RewClass()
                                        {
                                            name1 = username.ToString(),
                                            rewiew1 = rewiew.ToString()
                                        });
                                    }
                                }
                            }
                            reader2.Close();
                        }
                    }
                }
                reader.Close();
            }
            RewList.ItemsSource = Rews;
        }

        private void LeaveRew(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hello");
        }
    }
}
