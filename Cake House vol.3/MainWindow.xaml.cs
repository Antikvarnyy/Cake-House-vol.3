using System;
using System.Collections.Generic;
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
            if(username.Content == "CakeHouse")
            {
                username.Content = "Log In, please";
                enter = 0;
                log.Content = "Log In";
            }
        }
    }
}
