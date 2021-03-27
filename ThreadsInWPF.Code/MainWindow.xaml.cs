using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ThreadsInWPF.Code
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _score = 0;

        
        public MainWindow()
        {
            InitializeComponent();
            Thread orderThreads = new Thread(GenerateOrder);
            orderThreads.Start();
        }

        private void BtnPutIn1_Click(object sender, RoutedEventArgs e)
        {
            if (lbFruits.SelectedItem != null)
            {
                var fruit = (lbFruits.SelectedItem as ListBoxItem).Content;
                lbBlender1.Items.Add(new ListBoxItem { Content = fruit });

                btnEmpty1.IsEnabled = true;
            }
        }

        private void BtnPutIn2_Click(object sender, RoutedEventArgs e)
        {
            if (lbFruits.SelectedItem != null)
            {
                var fruit = (lbFruits.SelectedItem as ListBoxItem).Content;
                lbBlender2.Items.Add(new ListBoxItem { Content = fruit });

                btnEmpty2.IsEnabled = true;
            }
        }

        private void BtnBlend1_Click(object sender, RoutedEventArgs e)
        {
            btnPutIn1.IsEnabled = false;

            Thread blendThread1 = new Thread(Blend1);
            blendThread1.Start(); 
        }

        private void BtnBlend2_Click(object sender, RoutedEventArgs e)
        {
            btnPutIn2.IsEnabled = false;

            Thread blendThread2 = new Thread(Blend2);
            blendThread2.Start();
        }

        private void Blend1()
        {
            btnEmpty1.Dispatcher.Invoke(new Action(() => btnEmpty1.IsEnabled = false));

            int blendTime = 10;
            for (int i = 0; i < blendTime; i++)
            {
                pbBlend1.Dispatcher.Invoke(new Action(() => pbBlend1.Value += 10));
                tbStatus1.Dispatcher.Invoke(new Action(() => tbStatus1.Text = $"Blending {i}"));
                Thread.Sleep(1000);
            }
            tbStatus1.Dispatcher.Invoke(new Action(() => tbStatus1.Text= "Juice Ready"));
            btnPutIn1.Dispatcher.Invoke(new Action(() => btnPutIn1.IsEnabled = true));
            btnEmpty1.Dispatcher.Invoke(new Action(() => btnEmpty1.IsEnabled = true));
            pbBlend1.Dispatcher.Invoke(new Action(() => pbBlend1.Value = 0));

        }

        private void Blend2()
        {
            btnEmpty2.Dispatcher.Invoke(new Action(() => btnEmpty2.IsEnabled = false));

            int blendTime = 10;
            for (int i = 0; i < blendTime; i++)
            {
                pbBlend2.Dispatcher.Invoke(new Action(() => pbBlend2.Value += 10));
                tbStatus2.Dispatcher.Invoke(new Action(() => tbStatus2.Text = $"Blending {i}"));
                Thread.Sleep(1000);
            }
            tbStatus2.Dispatcher.Invoke(new Action(() => tbStatus2.Text = "Juice Ready"));
            btnPutIn2.Dispatcher.Invoke(new Action(() => btnPutIn2.IsEnabled = true));
            btnEmpty2.Dispatcher.Invoke(new Action(() => btnEmpty2.IsEnabled = true));
            pbBlend2.Dispatcher.Invoke(new Action(() => pbBlend2.Value = 0));

        }

        private void BtnCleanBlend1_Click(object sender, RoutedEventArgs e)
        {
            tbStatus1.Text = "Cleaned";
            btnCleanBlend1.IsEnabled = false;
            btnPutIn1.IsEnabled = true;
        }

        private void BtnCleanBlend2_Click(object sender, RoutedEventArgs e)
        {
            tbStatus2.Text = "Cleaned";
            btnCleanBlend2.IsEnabled = false;
            btnPutIn2.IsEnabled = true;
        }

        private void BtnEmpty2_Click(object sender, RoutedEventArgs e)
        {
            List<ListBoxItem> currentOrders = new List<ListBoxItem>();

            foreach (ListBoxItem item in lbOrders.Items)
            {
                currentOrders.Add(item);
            }

            CheckBlender2(currentOrders);

            lbBlender2.Items.Clear();
            tbStatus2.Text = "Dirty";
            btnCleanBlend2.IsEnabled = true;
            btnEmpty2.IsEnabled = false;
            btnPutIn2.IsEnabled = false;
            


        }

        private void BtnEmpty1_Click(object sender, RoutedEventArgs e)
        {
            List<ListBoxItem> currentOrders = new List<ListBoxItem>();

            foreach (ListBoxItem item in lbOrders.Items)
            {
                currentOrders.Add(item);
            }

            CheckBlender1(currentOrders);
            lbBlender1.Items.Clear();
            
            tbStatus1.Text = "Dirty";
            btnCleanBlend1.IsEnabled = true;
            btnEmpty1.IsEnabled = false;
            btnPutIn1.IsEnabled = false;

        }

        private void GenerateOrder()
        {
            var rand = new Random();

            StringBuilder builder = new StringBuilder();


            while (true)
            {
                string[] fruits = { "Apple", "Pear", "Banana", "Mango", "Orange", "Grape", "Peach", "Cherry", "Strawberry", "Plum", "Blackberry", "Raspberry", "Pineapple", "Kiwi" };
                int numberOfFruits = rand.Next(1, 5);

                for (int i = 0; i < numberOfFruits; i++)
                {
                    int fruit = rand.Next(1, 14);

                    //Skal måske ikke have mellemrum efter komma
                    builder.Append($"{fruits[fruit]}, ");

                }

                lbOrders.Dispatcher.Invoke(new Action(() => lbOrders.Items.Add(new ListBoxItem { Content = builder.ToString() })));
                builder.Clear();

                Thread.Sleep(10000);
            }
        }

        private void CheckBlender1(List<ListBoxItem> currentOrders)
        {
            
            StringBuilder blenderBuilder = new StringBuilder();

            foreach (ListBoxItem item in lbBlender1.Items)
            {
                blenderBuilder.Append($"{item.Content}, ");
            }

            foreach (ListBoxItem item in currentOrders)
            {
                if (item.Content.ToString() == blenderBuilder.ToString())
                {
                    lbOrders.Items.Remove(item);
                    _score++;
                    lblScore.Content = $"SCORE: {_score}";
                }
            }

        }

        private void CheckBlender2(List<ListBoxItem> currentOrders)
        {

            StringBuilder blenderBuilder = new StringBuilder();

            foreach (ListBoxItem item in lbBlender2.Items)
            {
                blenderBuilder.Append($"{item.Content}, ");
            }

            foreach (ListBoxItem item in currentOrders)
            {
                if (item.Content.ToString() == blenderBuilder.ToString())
                {
                    lbOrders.Items.Remove(item);
                    _score++;
                    lblScore.Content = $"SCORE: {_score}";
                }
            }

        }
    }
}
