using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        string user = "";
        public Orders(string username)
        {
            InitializeComponent();
            user = username;

            data.DisplayDateStart = DateTime.Now.AddDays(1);
            filling();
        }

        private async void filling()
        {
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string selcustom = "SELECT * FROM Orders";
                SqlCommand command = new SqlCommand(selcustom, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object orderdate = reader.GetValue(2);

                        DateTime dateToRemove = DateTime.ParseExact(orderdate.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                        data.BlackoutDates.Add(new CalendarDateRange(dateToRemove));
                    }
                }
            }

        }

        private async void conf(object sender, RoutedEventArgs e)
        {
            if(email.Text == "")
            {
                MessageBox.Show("Leave your e-mail so that our specialist can contact you to clarify and confirm the order", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if(details.Text == "")
            {
                MessageBox.Show("Leave some details about cake what you want", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";

            int idcustom = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string selcustom = "SELECT * FROM customers";
                SqlCommand command = new SqlCommand(selcustom, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while(await reader.ReadAsync())
                    {
                        object id_custom = reader.GetValue(0);
                        object name_custom = reader.GetValue(1);
                        if(name_custom.ToString() == user)
                        {
                            idcustom = Convert.ToInt32(id_custom);
                            break;
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string enter = $"INSERT INTO Orders VALUES({idcustom},'{data.Text}','{details.Text}')";
                SqlCommand commandcat = new SqlCommand(enter, connection);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
            MessageBox.Show("Success! A confirmation email will arrive in your email soon!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
