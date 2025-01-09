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

namespace Labo08_Chevalley_Michaud
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : UserControl
    {
        public Overview()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ThreadMachine.Instance.Connected) { ConnectElips.Fill = new SolidColorBrush(Colors.Green); }
            else { ConnectElips.Fill = new SolidColorBrush(Colors.Red); }
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ThreadMachine.Instance.Start = true;
        }
    }
}
