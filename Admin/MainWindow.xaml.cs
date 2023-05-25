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

namespace Admin
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
            if (sender.ToString().Contains("Add"))
            {
                add.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else if (sender.ToString().Contains("Edit cake"))
            {
                deledit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
            else if (sender.ToString().Contains("Edit order"))
            {
                orders.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }            
            else
            {
                profit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDF5D3"));
            }
        }
        private void Leave(object sender, MouseEventArgs e)
        {
            if (sender.ToString().Contains("Add"))
            {
                add.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else if (sender.ToString().Contains("Edit cake"))
            {
                deledit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
            else if (sender.ToString().Contains("Edit order"))
            {
                orders.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }            
            else
            {
                profit.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1BD77"));
            }
        }

        private void Addbutton(object sender, RoutedEventArgs e)
        {
            AddCakeWindow acw = new AddCakeWindow();
            Hide();
            acw.ShowDialog();
            Show();
        }

        private void DelCake(object sender, RoutedEventArgs e)
        {
            EditCakeWindow ecw = new EditCakeWindow();
            Hide();
            ecw.ShowDialog();
            Show();
        }

        private void DelOrder(object sender, RoutedEventArgs e)
        {
            EditOrderWindow eow = new EditOrderWindow();
            Hide();
            eow.ShowDialog();
            Show();
        }

        private void Check(object sender, RoutedEventArgs e)
        {

        }
    }
}
