using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
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
using System.Windows.Threading;

namespace Potoki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        int min;
        int max;
        int countPotok;
        string Maxi;
        string Mini;
        string avge;
        public MainWindow()
        {
            InitializeComponent();

        }

     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Thread thread = new Thread(CountNumbers);
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Diapazon()
        {
            if (minbox.Text != "" && maxbox.Text != "")
            {
                min = Convert.ToInt32(minbox.Text);
                max= Convert.ToInt32(maxbox.Text);
                int a;
                if(min>max)
                {
                    a = max;
                    max = min;
                    min = a;
                }
            }
            else
            {
                min = 0;
                max = 50;
            }
        }
        private void Count()
        {
            countPotok = Convert.ToInt32(countthread.Text);
        }
        private void CountNumbers()
        {
            try
            {
                for (int i = 0; i <= 50; i++)
                {
                    // Обновляем UI в потоке UI
                    Dispatcher.Invoke(() => boxinf.Text += i + "\n");
                    Thread.Sleep(100); 
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CountNumbersDiapazon()
        {
            try
            {

                //boxinf.Text = "";
                for (int i = min; i <= max; i++)
                {

                    // Обновляем UI в потоке UI
                    Dispatcher.Invoke(() => boxinf.Text += i + "\n");
                  
                        
                    Thread.Sleep(250);
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Diapazon();
                Thread thread2 = new Thread(CountNumbersDiapazon);
                thread2.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CountNumberAndThread()
        {
            try
            {
                Task[] tasks = new Task[countPotok];
                
                for (int i = 0; i < countPotok; i++)
                {
                    tasks[i] = Task.Factory.StartNew(() => CountNumbersDiapazon());
                }
                Task.WhenAll(tasks); // Ожидаем завершения всех задач
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
               
                Count();
                Diapazon();
                CountNumberAndThread();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteFile()
        {
            Thread fileThread = new Thread(() =>
            {
                using (StreamWriter writer = new StreamWriter("results.txt", true))
                {
                    writer.WriteLine("Max = " + Maxi);
                    writer.WriteLine("Min = " + Mini);
                    writer.WriteLine("Avg = " + avge);

                }
            });
            fileThread.Start();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                int[] numbers = new int[26];
                Random random = new Random();

                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = random.Next(1, 10001);
                    Dispatcher.Invoke(() => boxinf.Text += numbers[i] + "\n");
                }

                Thread maxThread = new Thread(() =>
                {
                    int max = numbers[0];
                    for (int i = 1; i < numbers.Length; i++)
                    {
                        if (numbers[i] > max)
                        {
                            max = numbers[i];
                        }
                    }
                    Dispatcher.Invoke(() => boxinf.Text += "Max = " + max + "\n");
                    Maxi = max.ToString();

                });
                maxThread.Start();

                Thread minThread = new Thread(() =>
                {
                    int min = numbers[0];
                    for (int i = 1; i < numbers.Length; i++)
                    {
                        if (numbers[i] < min)
                        {
                            min = numbers[i];
                        }
                    }
                    Dispatcher.Invoke(() => boxinf.Text += "Min = " + min + "\n");
                    Mini = min.ToString();
             
                });
                minThread.Start();

                Thread avgThread = new Thread(() =>
                {
                    int sum = 0;
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        sum += numbers[i];
                    }
                    double avg = (double)sum / numbers.Length;
                    
                    Dispatcher.Invoke(() => boxinf.Text += "Avg = " + avg + "\n");
                    
                    avge = avg.ToString();
                    //MessageBox.Show(avge);
                 
                });
                avgThread.Start();

                //WriteFile();


            }
            catch (Exception s)
            {
                MessageBox.Show(s.Message);
            }
            
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            WriteFile();
        }
    }
}
