using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace WpfApp1
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

        private async void EVENT_LOAD(object sender, RoutedEventArgs e)
        {
            linkload:
#pragma warning disable CS0618 // Тип или член устарел
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
#pragma warning restore CS0618 // Тип или член устарел
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipPoint = new IPEndPoint(ipAddress, 8005);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(ipPoint);

                socket.Listen(10);
                IP4CONNECT_LABEL.Content = "Server started on adress: " + ipAddress.ToString() + ":8005";
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        Socket handler = socket.Accept();
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;
                        byte[] data = new byte[256];

                        do
                        {
                            bytes = handler.Receive(data);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (handler.Available > 0);
                        Action action = () =>
                        {
                            MESSAGES.Text += DateTime.Now.ToShortTimeString() + ": " + builder.ToString() + "\n";
                        };
                        await MESSAGES.Dispatcher.BeginInvoke(action);
                        string message = "Your message sended";
                        data = Encoding.Unicode.GetBytes(message);
                        handler.Send(data);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Memate.BeginInit();
            }
        }
        private void SEND_CLICK(object sender, RoutedEventArgs e)
        {
            string ipAdress = IP_BOX.Text;
            int port = Convert.ToInt32(PORT_BOX.Text);
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                string message = MESSAGE_TEXTBOX.Text;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);
                MESSAGES.Text += DateTime.Now.ToShortTimeString() + ": " + message + "\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Memate.BeginInit();
            }
        }

        //private async void IP_BOX_TextChanged(object sender, TextChangedEventArgs e)
        //{
        ////    STATUS_CONNECT.Content = "Loading...";
        ////    if (PORT_BOX.Text == "")
        ////    {
        ////        STATUS_CONNECT.Content = "Enter port";
        ////        return;
        ////    }
        ////    else if (IP_BOX.Text == "")
        ////    {
        ////        STATUS_CONNECT.Content = "Enter IP";
        ////        return;
        ////    }
        ////    try
        ////    {
        ////        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(IP_BOX.Text), Convert.ToInt32(PORT_BOX.Text));

        ////        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ////        await Task.Run(() => socket.Connect(ipPoint));
        ////        STATUS_CONNECT.Content = "Connected";
        ////    }
        ////    catch (Exception)
        ////    {
        ////        STATUS_CONNECT.Content = "Not connected";
        ////    }
        //}

        //private async void PORT_BOX_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //STATUS_CONNECT.Content = "Loading...";
        //    //if (PORT_BOX.Text == "")
        //    //{
        //    //    STATUS_CONNECT.Content = "Enter port";
        //    //    return;
        //    //}
        //    //else if (IP_BOX.Text == "")
        //    //{
        //    //    STATUS_CONNECT.Content = "Enter IP";
        //    //    return;
        //    //}
        //    //try
        //    //{
        //    //    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(IP_BOX.Text), Convert.ToInt32(PORT_BOX.Text));

        //    //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    //    await Task.Run(() => socket.Connect(ipPoint));
        //    //    STATUS_CONNECT.Content = "Connected";
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    STATUS_CONNECT.Content = "Not connected";
        //    //}
        //}
    }
}