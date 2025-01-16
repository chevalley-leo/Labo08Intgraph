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
using System.Windows.Threading;

namespace Labo08_Chevalley_Michaud
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : UserControl
    {
        private DispatcherTimer updateTimer;
        public Overview()
        {

            InitializeComponent();
            // Initialize the timer
            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Set update interval (e.g., every second)
            };
            updateTimer.Tick += UpdateVisuals; // Attach event handler
            updateTimer.Start(); // Start the timer
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

        private void UpdateVisuals(object sender, EventArgs e)
        {
            // Update your UI elements here
            // Example: Update a label to show the current time
            if (ThreadMachine.Instance.Connected) { ConnectElips.Fill = new SolidColorBrush(Colors.Green); }
            else { ConnectElips.Fill = new SolidColorBrush(Colors.Red); }
            if(ThreadMachine.Instance.TotalBucketBatch>0)
                ProgressRingJob.Progress = ThreadMachine.Instance.NumberMadeBucket/ThreadMachine.Instance.TotalBucketBatch;
        }
    } 
}
