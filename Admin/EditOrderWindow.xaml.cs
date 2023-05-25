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

namespace Admin
{
    /// <summary>
    /// Логика взаимодействия для EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        public EditOrderWindow()
        {
            InitializeComponent();
            data.DisplayDateStart = DateTime.Now.AddDays(1);
            data.IsEnabled = false;
            nikname.IsEnabled = false;
            details.IsEnabled = false;
            save.IsEnabled = false;
            del.IsEnabled = false;
            filling();
        }
        string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
        private async void filling()
        {
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string custom = "SELECT * FROM Orders";
                SqlCommand command = new SqlCommand(custom, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object id = reader.GetValue(0);

                        Orders.Items.Add(id.ToString());
                    }
                    reader.Close();
                }
            }

        }
        private void Levitate(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("Save"))
                save.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            else
                del.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));

        }
        private void Leave(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("Save"))
                save.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            else
                del.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));

        }
        int user_ididi = -1;
        private async void change(object sender, SelectionChangedEventArgs e)
        {
            data.IsEnabled = true;
            details.IsEnabled = true;
            save.IsEnabled = true;
            del.IsEnabled = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string custom = "SELECT * FROM Orders";
                SqlCommand command = new SqlCommand(custom, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object id = reader.GetValue(0);
                        object userid = reader.GetValue(1);
                        object databd = reader.GetValue(2);
                        object detail = reader.GetValue(3);

                        using (SqlConnection connection1 = new SqlConnection(connectionString))
                        {
                            await connection1.OpenAsync();
                            string custom1 = "SELECT * FROM Customers";
                            SqlCommand command1 = new SqlCommand(custom1, connection1);
                            SqlDataReader reader1 = await command1.ExecuteReaderAsync();

                            if (reader1.HasRows)
                            {
                                while (await reader1.ReadAsync())
                                {
                                    object user_id = reader1.GetValue(0);
                                    object username = reader1.GetValue(1);

                                    if (Convert.ToInt32(user_id) == Convert.ToInt32(userid) && id.ToString() == Orders.SelectedItem.ToString())
                                    {
                                        user_ididi = Convert.ToInt32(userid);
                                        nikname.Text = username.ToString();
                                        details.Text = detail.ToString();
                                        DateTime dateToRemove = DateTime.ParseExact(databd.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);

                                        data.BlackoutDates.Add(new CalendarDateRange(dateToRemove));
                                    }
                                }
                                reader1.Close();
                            }
                        }
                    }
                    reader.Close();
                }
            }
        }

        private async void drop(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("You want to delete this order?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
                return;

            string del_bascet = $"DELETE FROM Orders WHERE id = '{Orders.SelectedItem.ToString()}'";
            using (SqlConnection connectioncat = new SqlConnection(connectionString))
            {
                await connectioncat.OpenAsync();
                SqlCommand commandcat = new SqlCommand(del_bascet, connectioncat);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
            Close();
        }

        private async void SaveB(object sender, RoutedEventArgs e)
        {
            if (data.Text == "")
            {
                MessageBox.Show("Choise a date for your order", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }            
            if (details.Text == "")
            {
                MessageBox.Show("Leave some details about cake what you want", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string del_bascet = $"DELETE FROM Orders WHERE id = '{Orders.SelectedItem.ToString()}'";
            using (SqlConnection connectioncat = new SqlConnection(connectionString))
            {
                await connectioncat.OpenAsync();
                SqlCommand commandcat = new SqlCommand(del_bascet, connectioncat);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string enter = $"INSERT INTO Orders VALUES({user_ididi},'{data.Text}','{details.Text}')";
                SqlCommand commandcat = new SqlCommand(enter, connection);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
            Close();
        }
    }
}