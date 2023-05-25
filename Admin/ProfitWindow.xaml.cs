using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
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
    /// Логика взаимодействия для ProfitWindow.xaml
    /// </summary>
    public partial class ProfitWindow : Window
    {
        public ProfitWindow()
        {
            InitializeComponent();
            filling();
        }
        public class CakeClass
        {
            public string name1 { get; set; }
            public BitmapImage picture1 { get; set; }
            public int count1 { get; set; }
            public int price1 { get; set; }
            public int total1 { get; set; }
        }
        public ObservableCollection<CakeClass> Cakes { get; set; }
        string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
        int total = 0;
        private async void filling()
        {
            Cakes = new ObservableCollection<CakeClass>();
            string catall = "SELECT * FROM SoldCakes";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(catall, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();


                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        object cakeid = reader.GetValue(1);
                        object soldcount = reader.GetValue(2);

                        string catall1 = "SELECT * FROM Cakes";
                        using (SqlConnection connection1 = new SqlConnection(connectionString))
                        {
                            await connection1.OpenAsync();

                            SqlCommand command1 = new SqlCommand(catall1, connection1);
                            SqlDataReader reader1 = await command1.ExecuteReaderAsync();

                            if (reader1.HasRows)
                            {
                                while (await reader1.ReadAsync())
                                {
                                    object id = reader1.GetValue(0);
                                    object name = reader1.GetValue(1);
                                    object picture = reader1.GetValue(3);
                                    object price = reader1.GetValue(7);

                                    if(Convert.ToInt32(id) == Convert.ToInt32(cakeid))
                                    {
                                        try
                                        {
                                            byte[] imageData = (byte[])picture;
                                            BitmapImage bitmapImage = new BitmapImage();
                                            using (MemoryStream memoryStream = new MemoryStream(imageData))
                                            {
                                                bitmapImage.BeginInit();
                                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Установка режима кэширования изображения
                                                bitmapImage.StreamSource = memoryStream;
                                                bitmapImage.EndInit();
                                            }

                                            Cakes.Add(new CakeClass()
                                            {
                                                name1 = name.ToString(),
                                                picture1 = bitmapImage,
                                                count1 = Convert.ToInt32(soldcount),
                                                price1 = Convert.ToInt32(price),
                                                total1 = Convert.ToInt32(soldcount) * Convert.ToInt32(price)
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                            Cakes.Add(new CakeClass()
                                            {
                                                name1 = name.ToString(),
                                                count1 = Convert.ToInt32(soldcount),
                                                price1 = Convert.ToInt32(price),
                                                total1 = Convert.ToInt32(soldcount) * Convert.ToInt32(price)
                                            });
                                        }
                                        total += Convert.ToInt32(soldcount) * Convert.ToInt32(price);
                                    }
                                }
                                reader1.Close();
                            }
                        }
                    }
                    reader.Close();
                }
            }
            CakeList.ItemsSource = Cakes;
            profit.Content = "Total: " + total + "₴";
        }
    }
}