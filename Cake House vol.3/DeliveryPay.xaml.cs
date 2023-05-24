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
using System.Windows.Shapes;

namespace Cake_House_vol._3
{
    /// <summary>
    /// Логика взаимодействия для DeliveryPay.xaml
    /// </summary>
    public partial class DeliveryPay : Window
    {
        string t = "";
        string username = "";
        public DeliveryPay(string total, string user)
        {
            InitializeComponent();
            t = total;
            username = user;
        }

        private void pay(object sender, RoutedEventArgs e)
        {
            if(address.Text == "")
            {
                MessageBox.Show("Enter your address for deliveryman!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Check check = new Check(address.Text, t, username);
            check.Owner = this;
            Hide();
            check.ShowDialog();
            Close();
        }
    }
}
