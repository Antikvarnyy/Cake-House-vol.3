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
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Levitate(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("Log"))
            {
                log.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else
            {
                sing.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
        }

        private void Leave(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("Log"))
            {
                log.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else
            {
                sing.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
        }

        private async void Logging(object sender, RoutedEventArgs e)
        {
            if (login.Text == "")
            {
                MessageBox.Show("Enter your login", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (pass.Password == "")
            {
                MessageBox.Show("Enter your password", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string custom = "SELECT * FROM customers";
                SqlCommand command = new SqlCommand(custom, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();


                if (reader.HasRows)
                {
                    int f = 0;
                    while (await reader.ReadAsync())
                    {
                        object username = reader.GetValue(1);
                        object userpass = reader.GetValue(2);
                        if (username.ToString() == login.Text)
                        {
                            f++;
                            if (userpass.ToString() == pass.Password)
                            {
                                MessageBox.Show("You enter as " + username);
                                reader.Close();
                                Owner.Name = username.ToString();
                                Hide();
                                Owner.Show();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Wrong password");
                                reader.Close();
                                return;
                            }
                        }
                    }
                    if (f == 0)
                    {
                        MessageBox.Show("Wrong login");
                    }
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("Wrong login");
                }
            }
        }
        private void Singing(object sender, RoutedEventArgs e)
        {
            Singer singer = new Singer();
            singer.Owner = this;
            Hide();
            singer.ShowDialog();
            Show();
        }
    }
}

