﻿using Microsoft.Win32;
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
    /// Логика взаимодействия для AddCakeWindow.xaml
    /// </summary>
    public partial class AddCakeWindow : Window
    {
        public AddCakeWindow()
        {
            InitializeComponent();
        }
        byte[] imageData = null;
        private void picload(object sender, RoutedEventArgs e)
        {
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
        private async void Createbutton(object sender, RoutedEventArgs e)
        {
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
            if(picpath.Content.ToString() == "")
            {
                MessageBoxResult result = MessageBox.Show("Add cake without picture?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    return;
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
