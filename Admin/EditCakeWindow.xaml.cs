using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
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
    /// Логика взаимодействия для EditCakeWindow.xaml
    /// </summary>
    public partial class EditCakeWindow : Window
    {
        public EditCakeWindow()
        {
            InitializeComponent();
            filling();
        }
        string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";

        private async void filling()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string custom = "SELECT * FROM Cakes";
                SqlCommand command = new SqlCommand(custom, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();


                if (reader.HasRows)
                {
                    int f = 0;
                    while (await reader.ReadAsync())
                    {
                        object cakename = reader.GetValue(1);

                        Cakes.Items.Add(cakename.ToString());
                    }
                    reader.Close();
                }
            }
        }
        byte[] imageData = null;
        private void picload(object sender, RoutedEventArgs e)
        {
            if (picpath.Content != "")
            {
                MessageBoxResult result = MessageBox.Show("This product already has a picture, want to replace?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images (*.png)|*.png|(*.jpeg)|*.jpeg|(*.jpg)|*.jpg";
            if (openFileDialog.ShowDialog() != true)
                return;
            string path = openFileDialog.FileName;
            string picname = "";
            foreach (char c in path)
            {
                picname += c;
                if (c == '\\')
                    picname = "";
            }
            picpath.Content = picname;

            Bitmap image = new Bitmap($"{path}");
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageData = ms.ToArray();
        }
        private async void picdel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("You want to delete image for this product?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
                return;
            string del_bascet = $"UPDATE Cakes set picture = NULL WHERE name = {Cakes.SelectedItem.ToString()}";
            using (SqlConnection connectioncat = new SqlConnection(connectionString))
            {
                await connectioncat.OpenAsync();
                SqlCommand commandcat = new SqlCommand(del_bascet, connectioncat);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
            picpath.Content = "";
            picdell.IsEnabled = false;
        }

        private async void change(object sender, SelectionChangedEventArgs e)
        {
            nameadd.IsEnabled = true;
            catadd.IsEnabled = true;
            weightadd.IsEnabled = true;
            countadd.IsEnabled = true;
            ingradd.IsEnabled = true;
            priceadd.IsEnabled = true;
            picadd.IsEnabled = true;
            picdell.IsEnabled = true;
            cakeadd.IsEnabled = true;
            cakedel.IsEnabled = true;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string custom = "SELECT * FROM Cakes";
                SqlCommand command = new SqlCommand(custom, connection);
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

                        using (SqlConnection connection1 = new SqlConnection(connectionString))
                        {
                            await connection1.OpenAsync();
                            string custom1 = "SELECT * FROM Categories";
                            SqlCommand command1 = new SqlCommand(custom1, connection1);
                            SqlDataReader reader1 = await command1.ExecuteReaderAsync();
                            if (reader1.HasRows)
                            {
                                while (await reader1.ReadAsync())
                                {
                                    object idcatg = reader1.GetValue(0);
                                    object namecatg = reader1.GetValue(1);

                                    if (Convert.ToInt32(idcatg) == Convert.ToInt32(category) && Cakes.SelectedItem.ToString() == name.ToString())
                                    {
                                        nameadd.Text = name.ToString();
                                        catadd.Text = namecatg.ToString();
                                        weightadd.Text = weight.ToString();
                                        countadd.Text = count.ToString();
                                        ingradd.Text = ingridients.ToString();
                                        priceadd.Text = price.ToString();
                                        if (picture == DBNull.Value)
                                        {
                                            picdell.IsEnabled = false;
                                        }
                                        else
                                        {
                                            picpath.Content = "SomePicture(need fix)";
                                        }
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
            MessageBoxResult result = MessageBox.Show("You want to delete this product?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
                return;
            nameadd.IsEnabled = false;
            catadd.IsEnabled = false;
            weightadd.IsEnabled = false;
            countadd.IsEnabled = false;
            ingradd.IsEnabled = false;
            priceadd.IsEnabled = false;
            picadd.IsEnabled = false;
            picdell.IsEnabled = false;
            cakeadd.IsEnabled = false;
            cakedel.IsEnabled = false;

            nameadd.Text = "";
            catadd.Text = "";
            countadd.Text = "";
            priceadd.Text = "";
            weightadd.Text = "";
            ingradd.Text = "";
            picpath.Content = "";

            string del_bascet = $"DELETE FROM Cakes WHERE name = '{Cakes.SelectedItem.ToString()}'";
            using (SqlConnection connectioncat = new SqlConnection(connectionString))
            {
                await connectioncat.OpenAsync();
                SqlCommand commandcat = new SqlCommand(del_bascet, connectioncat);
                int num = await commandcat.ExecuteNonQueryAsync();
            }

            Cakes.Items.Clear();
            filling();
        }
        private async void Createbutton(object sender, RoutedEventArgs e)
        {
            #region check
            if (nameadd.Text == "" || catadd.Text == "" || weightadd.Text == "" || countadd.Text == "" || ingradd.Text == "" || priceadd.Text == "")
            {
                MessageBox.Show("Enter cake details");
                return;
            }
            int f = 0;
            foreach (char c in weightadd.Text)
            {
                if (!Char.IsDigit(c))
                {
                    weightadd.Text = "";
                    f++;
                    break;
                }
            }
            foreach (char c in countadd.Text)
            {
                if (!Char.IsDigit(c))
                {
                    countadd.Text = "";
                    f++;
                    break;
                }
            }
            foreach (char c in priceadd.Text)
            {
                if (!Char.IsDigit(c))
                {
                    priceadd.Text = "";
                    f++;
                    break;
                }
            }
            if (f != 0)
            {
                MessageBox.Show("The information entered is not valid");
                return;
            }

            MessageBoxResult res = MessageBox.Show("Name: " + nameadd.Text + "\nCategory: " + catadd.Text + "\nWeight: " + weightadd.Text + "\nCount: " + countadd.Text + "\nIngridients: " + ingradd.Text + "Price: " + priceadd.Text + "Picture path: " + picpath.Content + "\n\nCorrect?", "Info", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.No)
                return;
            #endregion
            string connectionString = @"Data Source = USER-PC50; Initial Catalog = CakeHouse; Trusted_Connection=True";
            string catall = "SELECT * FROM CATEGORIES";
            int catid = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(catall, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    int ff = 0;
                    while (await reader.ReadAsync())
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(1);
                        catid = Convert.ToInt32(id);
                        if (name.ToString() == catadd.Text)
                        {
                            ff++;
                            break;
                        }
                    }
                    reader.Close();
                    if (ff == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("This category does not exist, create it?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.No)
                            return;
                        catid++;
                        string sqlExpressioncat = $"INSERT INTO Categories(name) VALUES('{catadd.Text}')";
                        using (SqlConnection connectioncat = new SqlConnection(connectionString))
                        {
                            await connectioncat.OpenAsync();
                            SqlCommand commandcat = new SqlCommand(sqlExpressioncat, connectioncat);
                            int num = await commandcat.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            string del_bascet = $"DELETE FROM Cakes WHERE name = '{Cakes.SelectedItem.ToString()}'";
            using (SqlConnection connectioncat = new SqlConnection(connectionString))
            {
                await connectioncat.OpenAsync();
                SqlCommand commandcat = new SqlCommand(del_bascet, connectioncat);
                int num = await commandcat.ExecuteNonQueryAsync();
            }
            if (picpath.Content.ToString() == "" || picpath.Content.ToString() == "SomePicture(need fix)")
            {
                string sqlExpression = $"INSERT INTO Cakes(name,categoryid,weight,Ingridients,count,price) VALUES('{nameadd.Text}',{catid},{weightadd.Text},'{ingradd.Text}',{countadd.Text},{priceadd.Text})";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int num = await command.ExecuteNonQueryAsync();
                }
            }
            else
            {
                string sqlExpression = "INSERT INTO Cakes(name, categoryid, weight, Ingridients, count, price, picture) VALUES(@Name, @CategoryID, @Weight, @Ingridients, @Count, @Price, @Picture)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        command.Parameters.AddWithValue("@Name", nameadd.Text);
                        command.Parameters.AddWithValue("@CategoryID", catid);
                        command.Parameters.AddWithValue("@Weight", weightadd.Text);
                        command.Parameters.AddWithValue("@Ingridients", ingradd.Text);
                        command.Parameters.AddWithValue("@Count", countadd.Text);
                        command.Parameters.AddWithValue("@Price", priceadd.Text);
                        command.Parameters.AddWithValue("@Picture", imageData);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            Close();
        }

    }
}