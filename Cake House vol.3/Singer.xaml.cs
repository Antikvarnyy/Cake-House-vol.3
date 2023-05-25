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
    /// Логика взаимодействия для Singer.xaml
    /// </summary>
    public partial class Singer : Window
    {
        public Singer()
        {
            InitializeComponent();
        }
        private void Levitate(object sender, MouseEventArgs e)
        {
            sing.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
        }

        private void Leave(object sender, MouseEventArgs e)
        {
            sing.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
        }

        private async void Singing(object sender, RoutedEventArgs e)
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
            if (pass2.Password == "")
            {
                MessageBox.Show("Enter your password again", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (pass.Password != pass2.Password)
            {
                MessageBox.Show("Password mismatch!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                pass2.Password = "";
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
                        if (username.ToString() == login.Text)
                        {
                            MessageBox.Show("This login is already in use", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                            login.Text = "";
                            return;
                        }
                    }
                    string enteruser = $"INSERT INTO customers VALUES('{login.Text}','{pass.Password}',0)";
                    using (SqlConnection connectioncat = new SqlConnection(connectionString))
                    {
                        await connectioncat.OpenAsync();
                        SqlCommand commandcat = new SqlCommand(enteruser, connectioncat);
                        int num = await commandcat.ExecuteNonQueryAsync();
                    }
                    MessageBox.Show("Success!");
                    Close();
                }
                else
                {
                    string enteruser = $"INSERT INTO customers VALUES('{login.Text}','{pass.Password}',0)";
                    using (SqlConnection connectioncat = new SqlConnection(connectionString))
                    {
                        await connectioncat.OpenAsync();
                        SqlCommand commandcat = new SqlCommand(enteruser, connectioncat);
                        int num = await commandcat.ExecuteNonQueryAsync();
                    }
                    MessageBox.Show("Success!");
                    Close();
                }
            }
        }
    }
}
    