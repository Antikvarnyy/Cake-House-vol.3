using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для SendRew.xaml
    /// </summary>
    public partial class SendRew : Window
    {
        string username = "";
        public SendRew(string user)
        {
            InitializeComponent();
            username = user;
        }

        private async void LeaveRew(object sender, RoutedEventArgs e)
        {
            if (rew.Text == "")
                return;
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            int iduser = -1;
            string finduser = $"SELECT * from customers";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(finduser, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();


                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object name = reader.GetValue(1);
                        object id = reader.GetValue(0);
                        if (name.ToString() == username)
                        {
                            iduser = Convert.ToInt32(id);
                            break;
                        }
                    }
                }
            }
            string leavecomment = $"INSERT INTO reviews VALUES({iduser},'{rew.Text}')";
            using (SqlConnection connectioncat = new SqlConnection(connectionString))
            {
                await connectioncat.OpenAsync();
                SqlCommand commandcat = new SqlCommand(leavecomment, connectioncat);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
            Close();
        }
    }
}
