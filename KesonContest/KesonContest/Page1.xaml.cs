﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KesonContest;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KesonContest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private static readonly Socket ClientSocket = new Socket
           (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 197;
        string fileData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");
        string fileResult = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "result.txt");
        public Page1()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
           
        }

        private void bt_Connect_Clicked(object sender, EventArgs e)
        {
            ConnectToServer();
            while(!ClientSocket.Connected)
            {

            }
            File.WriteAllText(fileData, "");    // Delete old data
            File.WriteAllText(fileResult, "");
            SendString("~~*data");
            RequestLoop();              // Replace new data
        }

        private async void bt_Skip_Clicked(object sender, EventArgs e)
        {
            ConnectToServer();
            
            await  Navigation.PushAsync(new Page2());
        }

        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    IPAddress address = IPAddress.Parse("10.12.20.25");
                    ClientSocket.Connect(address, PORT);

                }
                catch (SocketException)
                {
                    Console.WriteLine("Fail to Connect Server");
                }
            }
            
        }

        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            
        }

        private async void RequestLoop()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    string a = ReceiveResponse();
                }
            });

        }

        private string ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return null;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string a = Encoding.ASCII.GetString(data);
            Device.BeginInvokeOnMainThread(() =>
            {
                lb_data.Text = a;
                string b = a.Substring(a.Length - 6, 4);
                if (b == ".end")
                {
                    File.WriteAllText(fileData, a);
                    Navigation.PushAsync(new Page2());
                }
            });
            return a;
        }

    }
}