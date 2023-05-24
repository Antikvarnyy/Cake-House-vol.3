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
    /// Логика взаимодействия для Check.xaml
    /// </summary>
    public partial class Check : Window
    {
        string username = "";
        public Check(string address, string t, string user)
        {
            InitializeComponent();
            addressa.Content = address;
            Total.Content = t;
            filling();
            username = user;
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

                                                if (name_custom.ToString() == username)
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
            BD_Deleting();
        }
        private async void BD_Deleting()
        {
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

                                                if (name_custom.ToString() == username)
                                                {
                                                    if (id_custom.ToString() == customid.ToString() && id_cake.ToString() == cakeid.ToString())
                                                    {
                                                        string upd_cakes = $"UPDATE Cakes set count = {Convert.ToInt32(count_cake) - Convert.ToInt32(count_basket)} WHERE Id = {id_cake}";
                                                        string udp_user = $"UPDATE customers set Buyer = 1 WHERE Id = {Convert.ToInt32(id_custom)}";
                                                        string sold_cake = $"INSERT INTO SoldCakes VALUES('{id_cake.ToString()}',{Convert.ToInt32(count_basket)})";
                                                        string del_basket = $"DELETE FROM Basket WHERE id = {Convert.ToInt32(id_basket)}";
                                                        using (SqlConnection connection = new SqlConnection(connectionString))
                                                        {
                                                            await connection.OpenAsync();
                                                            SqlCommand command11 = new SqlCommand(upd_cakes, connection);
                                                            int num = await command11.ExecuteNonQueryAsync();
                                                           
                                                            SqlCommand command22 = new SqlCommand(udp_user, connection);
                                                            num = await command22.ExecuteNonQueryAsync();
                                                           
                                                            SqlCommand command33 = new SqlCommand(sold_cake, connection);
                                                            num = await command33.ExecuteNonQueryAsync();
                                                           
                                                            SqlCommand command44 = new SqlCommand(del_basket, connection);
                                                            num = await command44.ExecuteNonQueryAsync();
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
