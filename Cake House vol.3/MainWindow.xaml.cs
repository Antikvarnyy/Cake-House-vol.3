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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cake_House_vol._3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            fillingsearch();
            filling();
        }
        private async void fillingsearch()
        {
            Price.Items.Add("Price");
            Price.Items.Add("<100");
            Price.Items.Add("100-500");
            Price.Items.Add("500-1000");
            Price.Items.Add("1000+");

            catg.Items.Add("Categoria");

            Weight.Items.Add("Weight");
            Weight.Items.Add("<200g");
            Weight.Items.Add("0,5kg");
            Weight.Items.Add("1-2kg");
            Weight.Items.Add("2kg+");

            Price.SelectedIndex = 0;
            catg.SelectedIndex = 0;
            Weight.SelectedIndex = 0;

            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            string catall = "SELECT * FROM Categories";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(catall, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();


                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object name = reader.GetValue(1);
                        catg.Items.Add(name);
                    }
                    reader.Close();
                }

            }
        }
        public class CakeClass
        {
            public int id1 { get; set; }
            public string name1 { get; set; }
            public string category1 { get; set; }
            public string picture1 { get; set; }
            public int weight1 { get; set; }
            public string Ingridients1 { get; set; }
            public int count1 { get; set; }
            public int price1 { get; set; }
        }
        public ObservableCollection<CakeClass> Cakes { get; set; }
        private async void filling()
        {
            Cakes = new ObservableCollection<CakeClass>();
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            string catall = "SELECT * FROM Cakes";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(catall, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();


                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(1);
                        object category = reader.GetValue(2);
                        object picture = reader.GetValue(3);
                        object weight = reader.GetValue(4);
                        object ingridients = reader.GetValue(5);
                        object count = reader.GetValue(6);
                        object price = reader.GetValue(7);
                        if (Price.SelectedIndex == 1 && Convert.ToInt32(price) > 100)
                            continue;
                        if (Price.SelectedIndex == 2 && (Convert.ToInt32(price) < 100 || Convert.ToInt32(price) > 499))
                            continue;
                        if (Price.SelectedIndex == 3 && (Convert.ToInt32(price) < 500 || Convert.ToInt32(price) > 999))
                            continue;
                        if (Price.SelectedIndex == 4 && Convert.ToInt32(price) < 1000)
                            continue;

                        if (catg.SelectedIndex != 0)
                        {
                            int f = 0;
                            string categ = "SELECT * FROM Categories";
                            using (SqlConnection connection2 = new SqlConnection(connectionString))
                            {
                                await connection2.OpenAsync();

                                SqlCommand command2 = new SqlCommand(categ, connection2);
                                SqlDataReader reader2 = await command2.ExecuteReaderAsync();


                                if (reader2.HasRows)
                                {
                                    while (await reader2.ReadAsync())
                                    {
                                        object id2 = reader2.GetValue(0);
                                        object category2 = reader2.GetValue(1);

                                        if (category.ToString() == id2.ToString())
                                        {
                                            if (catg.SelectedItem.ToString() != category2.ToString())
                                                f = 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (f == 1)
                                continue;
                        }
                        if (Convert.ToInt32(count) < 1)
                            continue;
                        if (Weight.SelectedIndex == 1 && Convert.ToInt32(weight) > 200)
                            continue;
                        if (Weight.SelectedIndex == 2 && (Convert.ToInt32(weight) < 200) || Convert.ToInt32(weight) > 999)
                            continue;
                        if (Weight.SelectedIndex == 3 && (Convert.ToInt32(weight) < 1000) || Convert.ToInt32(weight) > 1999)
                            continue;
                        if (Weight.SelectedIndex == 4 && Convert.ToInt32(weight) < 2000)
                            continue;

                        if (search.Text != "Search...")
                        {
                            if (!name.ToString().ToLower().Contains(search.Text.ToLower()) && search.Text.Length > 0)
                                continue;
                        }


                        Cakes.Add(new CakeClass()
                        {
                            id1 = Convert.ToInt32(id),
                            name1 = name.ToString(),
                            category1 = category.ToString(),
                            picture1 = picture.ToString(),
                            weight1 = Convert.ToInt32(weight),
                            Ingridients1 = ingridients.ToString(),
                            count1 = Convert.ToInt32(count),
                            price1 = Convert.ToInt32(price)
                        });
                    }
                    reader.Close();
                }
                CakeList.ItemsSource = Cakes;
            }
        }

        private void Levitate(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("About"))
            {
                about.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else if (sender.ToString().Contains("Our"))
            {
                reviews.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else if (sender.ToString().Contains("Order"))
            {
                order.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else if (sender.ToString().Contains("Log"))
            {
                log.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else
            {
                bracket.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
        }
        private void Leave(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("About"))
            {
                about.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else if (sender.ToString().Contains("Our"))
            {
                reviews.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else if (sender.ToString().Contains("Order"))
            {
                order.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else if (sender.ToString().Contains("Log"))
            {
                log.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else
            {
                bracket.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
        }
        int enter = 0;
        private void LogClick(object sender, RoutedEventArgs e)
        {
            if (log.Content == "Log out")
            {
                MessageBoxResult res = MessageBox.Show("You want to log out?", "Warning", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.No)
                    return;
                username.Content = "Log In, please";
                enter = 0;
                log.Content = "Log In";
            }
            else
            {
                Register reg = new Register();
                reg.Owner = this;
                Hide();
                reg.ShowDialog();
                username.Content = Name;
                Name = "Cake_House";
                reg.Close();
                Show();
                enter = 1;
                if (username.Content == "")
                    LogClick(sender, e);
                log.Content = "Log out";
            }
            if (username.Content == "CakeHouse")
            {
                username.Content = "Log In, please";
                enter = 0;
                log.Content = "Log In";
            }
        }
        private void click(object sender, KeyEventArgs e)
        {
            if (search.Text == "Search...")
            {
                search.Foreground = Brushes.Black;
                search.Text = "";
            }
            if (Cakes != null)
            {
                Cakes.Clear();
                filling();
            }
        }
        private void ChangeSelection(object sender, SelectionChangedEventArgs e)
        {
            if (Cakes != null)
            {
                Cakes.Clear();
                filling();
            }
        }
        private async void BuyClick(object sender, RoutedEventArgs e)
        {
            if (log.Content == "Log out" || log.Content == "CakeHouse") { }
            else
            {
                LogClick(sender, e);
                return;
            }

            Button button = (Button)sender;
            CakeClass cake = (CakeClass)button.DataContext;
            int index = Cakes.IndexOf(cake);
            if (index >= 0)
            {
                CakeList.SelectedIndex = index;
            }
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            int id = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string custom = "SELECT * FROM customers";
                SqlCommand command = new SqlCommand(custom, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object Id = reader.GetValue(0);
                        object name = reader.GetValue(1);
                        if (name.ToString() == username.Content.ToString())
                        {
                            id = Convert.ToInt32(Id);
                            break;
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string enter = $"INSERT INTO Basket VALUES('{cake.id1}','{id}',1)";
                SqlCommand commandcat = new SqlCommand(enter, connection);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
        }

        private void ToBasket(object sender, RoutedEventArgs e)
        {
            if (log.Content == "Log out" || log.Content == "CakeHouse") { }
            else
            {
                LogClick(sender, e);
                return;
            }
            Basket basket = new Basket(username.Content.ToString());
            basket.Owner = this;
            basket.Name = username.Content.ToString();
            Hide();
            basket.ShowDialog();
            Show();
            filling();
        }

        private void GoReviews(object sender, RoutedEventArgs e)
        {
            Reviews rew = new Reviews(username.Content.ToString());
            rew.Owner = this;
            Hide();
            rew.ShowDialog();
            Show();
        }

        private void GoOrders(object sender, RoutedEventArgs e)
        {
            if (log.Content == "Log out" || log.Content == "CakeHouse") { }
            else
            {
                LogClick(sender, e);
                return;
            }
            Orders orders = new Orders(username.Content.ToString());
            orders.Owner = this;
            Hide();
            orders.ShowDialog();
            Show();
        }
    }
}
