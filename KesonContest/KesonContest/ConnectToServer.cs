using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KesonContest
{
    /* public class LinkServer
    {
        
        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 197;

        public bool CheckConnect()
        {
            if(ClientSocket.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Main()
        {
            ConnectToServer();
            RequestLoop();
           // Exit();
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
                }
            }

        }


        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }

        public void SendRequest(string text)
        {
            //Console.Write("Send a request: ");
           // string request = Console.ReadLine();
            SendString(text);

            //if (request.ToLower() == "exit")
            //{
            //    Exit();
            //}
        }

        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        public async void RequestLoop()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    bool b = CheckConnect();
                    string h = ReceiveResponse();
                }
            });

        }

        public string ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return null;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string a =  Encoding.ASCII.GetString(data);
            Console.Write("Class Server Receive:");
            Console.Write(a);
            return a;
        }
    }
    */
}
