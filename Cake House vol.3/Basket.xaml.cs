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
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        public Basket(string name)
        {
            InitializeComponent();
            username.Content = name;
            filling();
        }
        private void Levitate(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("About"))
            {
                back.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else
            {
                Buy.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
        }

        private void Leave(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("About"))
            {
                back.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else
            {
                Buy.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
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
            public int[] count1 { get; set; }
            public int price1 { get; set; }
            public int selection1 { get; set; }
        }
        public ObservableCollection<CakeClass> Cakes { get; set; }
        private async void filling()
        {
            int total = 0;
            Cakes = new ObservableCollection<CakeClass>();
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            string selcakes = "SELECT * FROM Cakes";
            string selBasket = "SELECT * FROM Basket";
            string selcustom = "SELECT * FROM customers";
            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString))
                {
                    using (SqlConnection connection3 = new SqlConnection(connectionString))
                    {
                        await connection1.OpenAsync();
                        await connection2.OpenAsync();
                        await connection3.OpenAsync();

                        SqlCommand command3 = new SqlCommand(selcustom, connection3);
                        SqlDataReader reader3 = await command3.ExecuteReaderAsync();

                        if (reader3.HasRows)
                        {
                            while (await reader3.ReadAsync())
                            {
                                object id_custom = reader3.GetValue(0);
                                object name_custom = reader3.GetValue(1);

                                SqlCommand command2 = new SqlCommand(selBasket, connection2);
                                SqlDataReader reader2 = await command2.ExecuteReaderAsync();

                                if (reader2.HasRows)
                                {
                                    while (await reader2.ReadAsync())
                                    {
                                        object id_basket = reader2.GetValue(0);
                                        object cakeid = reader2.GetValue(1);
                                        object customid = reader2.GetValue(2);
                                        object count_basket = reader2.GetValue(3);

                                        SqlCommand command1 = new SqlCommand(selcakes, connection1);
                                        SqlDataReader reader1 = await command1.ExecuteReaderAsync();

                                        if (reader1.HasRows)
                                        {
                                            while (await reader1.ReadAsync())
                                            {
                                                object id_cake = reader1.GetValue(0);
                                                object name_cake = reader1.GetValue(1);
                                                object category_cake = reader1.GetValue(2);
                                                object picture_cake = reader1.GetValue(3);
                                                object weight_cake = reader1.GetValue(4);
                                                object ingridients_cake = reader1.GetValue(5);
                                                object count_cake = reader1.GetValue(6);
                                                object price_cake = reader1.GetValue(7);

                                                if (name_custom.ToString() == username.Content.ToString())
                                                {
                                                    if (id_custom.ToString() == customid.ToString() && id_cake.ToString() == cakeid.ToString())
                                                    {
                                                        int[] count = new int[Convert.ToInt32(count_cake)];
                                                        for (int i = 0; i < Convert.ToInt32(Convert.ToInt32(count_cake)); i++)
                                                        {
                                                            count[i] = i;
                                                        }
                                                        Cakes.Add(new CakeClass()
                                                        {
                                                            id1 = Convert.ToInt32(id_cake),
                                                            name1 = name_cake.ToString(),
                                                            category1 = category_cake.ToString(),
                                                            picture1 = picture_cake.ToString(),
                                                            weight1 = Convert.ToInt32(weight_cake),
                                                            count1 = count,
                                                            Ingridients1 = ingridients_cake.ToString(),
                                                            price1 = Convert.ToInt32(price_cake) * Convert.ToInt32(count_basket),
                                                            selection1 = Convert.ToInt32(count_basket)
                                                        });
                                                        total += Convert.ToInt32(price_cake) * Convert.ToInt32(count_basket);
                                                    }
                                                }
                                            }
                                        }
                                        reader1.Close();
                                    }
                                }
                                reader2.Close();
                            }
                        }
                        reader3.Close();
                    }
                }
            }
            CakeList.ItemsSource = Cakes;
            Total.Content = "Total: " + total + "₴";
        }

        private async void CountChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ListBoxItem listBoxItem = GetListBoxItem(comboBox);

            if (listBoxItem != null)
            {
                int index = CakeList.ItemContainerGenerator.IndexFromContainer(listBoxItem);
                if (comboBox.SelectedItem == null)
                    return;
                string selectedValue = comboBox.SelectedItem.ToString();

                CakeClass selectedItem = (CakeClass)CakeList.Items[index];
                string name = selectedItem.name1;

                string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
                string selcakes = "SELECT * FROM Cakes";
                string selBasket = "SELECT * FROM Basket";
                string selcustom = "SELECT * FROM customers";
                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {
                    using (SqlConnection connection2 = new SqlConnection(connectionString))
                    {
                        using (SqlConnection connection3 = new SqlConnection(connectionString))
                        {
                            await connection1.OpenAsync();
                            await connection2.OpenAsync();
                            await connection3.OpenAsync();

                            SqlCommand command3 = new SqlCommand(selcustom, connection3);
                            SqlDataReader reader3 = await command3.ExecuteReaderAsync();

                            if (reader3.HasRows)
                            {
                                while (await reader3.ReadAsync())
                                {
                                    object id_custom = reader3.GetValue(0);
                                    object name_custom = reader3.GetValue(1);

                                    SqlCommand command2 = new SqlCommand(selBasket, connection2);
                                    SqlDataReader reader2 = await command2.ExecuteReaderAsync();

                                    if (reader2.HasRows)
                                    {
                                        while (await reader2.ReadAsync())
                                        {
                                            object id_basket = reader2.GetValue(0);
                                            object cakeid = reader2.GetValue(1);
                                            object customid = reader2.GetValue(2);
                                            object count_basket = reader2.GetValue(3);

                                            SqlCommand command1 = new SqlCommand(selcakes, connection1);
                                            SqlDataReader reader1 = await command1.ExecuteReaderAsync();

                                            if (reader1.HasRows)
                                            {
                                                while (await reader1.ReadAsync())
                                                {
                                                    object id_cake = reader1.GetValue(0);
                                                    object name_cake = reader1.GetValue(1);
                                                    object category_cake = reader1.GetValue(2);
                                                    object picture_cake = reader1.GetValue(3);
                                                    object weight_cake = reader1.GetValue(4);
                                                    object ingridients_cake = reader1.GetValue(5);
                                                    object count_cake = reader1.GetValue(6);
                                                    object price_cake = reader1.GetValue(7);

                                                    if (name_custom.ToString() == username.Content.ToString())
                                                    {
                                                        if (id_custom.ToString() == customid.ToString() && id_cake.ToString() == cakeid.ToString())
                                                        {
                                                            if (name_cake.ToString() == name)
                                                            {
                                                                if (selectedValue == "0")
                                                                {
                                                                    string del_bascet = $"DELETE FROM Basket WHERE Id = {id_basket}";
                                                                    using (SqlConnection connectioncat = new SqlConnection(connectionString))
                                                                    {
                                                                        await connectioncat.OpenAsync();
                                                                        SqlCommand commandcat = new SqlCommand(del_bascet, connectioncat);
                                                                        int num = await commandcat.ExecuteNonQueryAsync();
                                                                    }
                                                                    filling();
                                                                }
                                                                else
                                                                {
                                                                    string del_bascet = $"UPDATE Basket set count = {selectedValue} WHERE Id = {id_basket}";
                                                                    using (SqlConnection connectioncat = new SqlConnection(connectionString))
                                                                    {
                                                                        await connectioncat.OpenAsync();
                                                                        SqlCommand commandcat = new SqlCommand(del_bascet, connectioncat);
                                                                        int num = await commandcat.ExecuteNonQueryAsync();
                                                                    }
                                                                    filling();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                reader1.Close();
                                            }
                                        }
                                        reader2.Close();
                                    }
                                }
                                reader3.Close();
                            }
                        }
                    }
                }
                
            }
        }

        private ListBoxItem GetListBoxItem(DependencyObject element)
        {
            while (element != null && !(element is ListBoxItem))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as ListBoxItem;
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
