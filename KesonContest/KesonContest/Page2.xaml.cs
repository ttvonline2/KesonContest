using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KesonContest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        #region Variable
        string fileData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");
        string fileResult = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "result.txt");
        String[] St_Data = new String[40];
        String[,] St_Result = new String[9,7];
        String AllResult = "0-1-28-26-36-90-|1-1-28-26-40-94-|2-1-26-26-38-90-|3-0-0-0-0-0-|4-1-28-28-40-96-|5-0-0-0-0-0-|6-0-0-0-0-0-|7-0-0-0-0-0-|";
        string st_HexColorOrange = "#ffb400";
        string st_HexColorGreen = "#30f000";
        string st_HexColorBlue = "#195e83";
        string st_HexColorSave0 = "#018fff";
        string st_HexcolorScore0 = "#c5c6ff";
        private static readonly Socket ClientSocket = new Socket
          (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        String TextReceive;
        String st_Stream = "";
        private const int PORT = 197;
        int int_step = 0;
        int int_CurrentShop = 0;
        int[] int_score = new int[40];
        int[,] int_sumScore = new int[8,4];
        #endregion

        public Page2()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        #region Page1
        private void bt_Connect_Clicked(object sender, EventArgs e)
        {
            ConnectToServer();
            while (!ClientSocket.Connected)
            {

            }
            File.WriteAllText(fileData, "");    // Delete old data
            File.WriteAllText(fileResult, "");
            TextReceive = ""; st_Stream = "";
            int_step = 0;
            SendString("~~*data");
            RequestLoop();              // Replace new data

        }

        private void bt_Skip_Clicked(object sender, EventArgs e)
        {
            int_step++;
            ConnectToServer();
            while (!ClientSocket.Connected)
            {

            }
            st_Stream = "";
            SendString("~~*stream");
            RequestLoop();              // Replace new data
            bt_Connect.IsVisible = false;
            bt_Skip.IsVisible = false;
            gr_page2.IsVisible = true;

            unfixSet();
            SetButtonShop(b_2, f_2, 0);
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

        #endregion

        #region Defaul

        void UnfixData()
        {
            String AllData = File.ReadAllText(fileData);
            string[] line = AllData.Split('|');
            for (int i = 0; i < line.Length; i++)
            {
                St_Data[i] = line[i];
                string a = St_Data[i];
            }
            for (int i = 27; i < 39; i++)
            {
                int_score[i] = Convert.ToInt16(St_Data[i]);
            }

        }
        void unfixSet()
        {
            UnfixData();
            t_0.Text = St_Data[0];
            t_1.Text = St_Data[1];
            t_2.Text = St_Data[2];
            t_3.Text = St_Data[3];
            t_4.Text = St_Data[4];
            t_5.Text = St_Data[5];
            t_6.Text = St_Data[6];
            t_7.Text = St_Data[7];
            t_8.Text = St_Data[8];
            t_9.Text = St_Data[9];
            t_10To17.Text = St_Data[10];             //Change
            t_18.Text = St_Data[18];
            t_19.Text = St_Data[19];
            t_20.Text = St_Data[20];
            t_21.Text = St_Data[21];
            t_22.Text = St_Data[22];
            t_23.Text = St_Data[23];
            t_24.Text = St_Data[24];
            t_25.Text = St_Data[25];
            t_26.Text = St_Data[26];
            t_27.Text = St_Data[27];
            t_28.Text = St_Data[28];
            t_29.Text = St_Data[29];
            t_30.Text = St_Data[30];
            t_31.Text = St_Data[31];
            t_32.Text = St_Data[32];
            t_33.Text = St_Data[33];
            t_34.Text = St_Data[34];
            t_35.Text = St_Data[35];
            t_36.Text = St_Data[36];
            t_37.Text = St_Data[37];
            t_38.Text = St_Data[38];
        }
        #endregion

        #region Stream DATA

        void inputVar()
        {

            //String AllResult = File.ReadAllText(fileResult);
            string[] li = AllResult.Split('|');
            for (int i = 0; i < li.Length; i++)
            {
                string[] n = li[i].Split('-');
                for (int j = 0; j < n.Length; j++)
                {
                    St_Result[i, j] = n[j];
                }
            }
        }

        void ColorButton()
        {
            if (St_Result[0, 1] == "0")
            {
                f_2.BackgroundColor = Color.FromHex(st_HexColorBlue); c_2.IsVisible = false;
            }
            else
            {
                f_2.BackgroundColor = Color.FromHex(st_HexColorOrange); c_2.IsVisible = true;
            }

            if (St_Result[1, 1] == "0")
            {
                f_3.BackgroundColor = Color.FromHex(st_HexColorBlue); c_3.IsVisible = false;
            }
            else
            {
                f_3.BackgroundColor = Color.FromHex(st_HexColorOrange); c_3.IsVisible = true;
            }

            if (St_Result[2, 1] == "0")
            {
                f_4.BackgroundColor = Color.FromHex(st_HexColorBlue); c_4.IsVisible = false;
            }
            else
            {
                f_4.BackgroundColor = Color.FromHex(st_HexColorOrange); c_4.IsVisible = true;
            }

            if (St_Result[3, 1] == "0")
            {
                f_5.BackgroundColor = Color.FromHex(st_HexColorBlue); c_5.IsVisible = false;
            }
            else
            {
                f_5.BackgroundColor = Color.FromHex(st_HexColorOrange); c_5.IsVisible = true;
            }

            if (St_Result[4, 1] == "0")
            {
                f_6.BackgroundColor = Color.FromHex(st_HexColorBlue); c_6.IsVisible = false;
            }
            else
            {
                f_6.BackgroundColor = Color.FromHex(st_HexColorOrange); c_6.IsVisible = true;
            }

            if (St_Result[5, 1] == "0")
            {
                f_7.BackgroundColor = Color.FromHex(st_HexColorBlue); c_7.IsVisible = false;
            }
            else
            {
                f_7.BackgroundColor = Color.FromHex(st_HexColorOrange); c_7.IsVisible = true;
            }

            if (St_Result[6, 1] == "0")
            {
                f_8.BackgroundColor = Color.FromHex(st_HexColorBlue); c_8.IsVisible = false;
            }
            else
            {
                f_8.BackgroundColor = Color.FromHex(st_HexColorOrange); c_8.IsVisible = true;
            }

            if (St_Result[7, 1] == "0")
            {
                f_9.BackgroundColor = Color.FromHex(st_HexColorBlue); c_9.IsVisible = false;
            }
            else
            {
                f_9.BackgroundColor = Color.FromHex(st_HexColorOrange); c_9.IsVisible = true;
            }

        }
        void SetButtonShop(Button _bt, Frame _fr, int stt)
        {

            int_CurrentShop = stt;
            inputVar();
            //Update other shop
            ColorButton();


            
            //Change Theme Name
            t_10To17.Text = St_Data[10 + stt];

            //Reset color Sorce

            t_27.BackgroundColor = Color.FromHex("#c5c6ff");
            t_28.BackgroundColor = Color.FromHex("#c5c6ff");
            t_29.BackgroundColor = Color.FromHex("#c5c6ff");
            t_30.BackgroundColor = Color.FromHex("#c5c6ff");

            t_31.BackgroundColor = Color.FromHex("#c5c6ff");
            t_32.BackgroundColor = Color.FromHex("#c5c6ff");
            t_33.BackgroundColor = Color.FromHex("#c5c6ff");
            t_34.BackgroundColor = Color.FromHex("#c5c6ff");

            t_35.BackgroundColor = Color.FromHex("#c5c6ff");
            t_36.BackgroundColor = Color.FromHex("#c5c6ff");
            t_37.BackgroundColor = Color.FromHex("#c5c6ff");
            t_38.BackgroundColor = Color.FromHex("#c5c6ff");

            // Reset Text at Save Button
            SumScore();
            bt_save.BackgroundColor = Color.FromHex(st_HexColorSave0);

            //Change color score of shop
            if (St_Result[stt, 1] == "1")
            {
                _bt.IsVisible = true;
                bt_save.BackgroundColor = Color.FromHex(st_HexColorOrange);
                
                //c1
                if (St_Result[stt, 2] == St_Data[27])
                {
                    t_27.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[28])
                {
                    t_28.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[29])
                {
                    t_29.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[30])
                {
                    t_30.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    t_27.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_28.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_29.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_30.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c2

                if (St_Result[stt, 3] == St_Data[31])
                {
                    t_31.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[32])
                {
                    t_32.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[33])
                {
                    t_33.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[34])
                {
                    t_34.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    t_31.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_32.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_33.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_34.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c3

                if (St_Result[stt, 4] == St_Data[35])
                {
                    t_35.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 4] == St_Data[36])
                {
                    t_36.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 4] == St_Data[37])
                {
                    t_37.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 4] == St_Data[38])
                {
                    t_38.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    t_35.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_36.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_37.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_38.BackgroundColor = Color.FromHex("#c5c6ff");
                }


            }
            else
            {
                _fr.BackgroundColor = Color.FromHex(st_HexColorGreen);
                //c1
                if (St_Result[stt, 2] == St_Data[27])
                {
                    t_27.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[28])
                {
                    t_28.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[29])
                {
                    t_29.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[30])
                {
                    t_30.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    t_27.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_28.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_29.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_30.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c2

                if (St_Result[stt, 3] == St_Data[31])
                {
                    t_31.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[32])
                {
                    t_32.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[33])
                {
                    t_33.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[34])
                {
                    t_34.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    t_31.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_32.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_33.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_34.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c3

                if (St_Result[stt, 4] == St_Data[35])
                {
                    t_35.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 4] == St_Data[36])
                {
                    t_36.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 4] == St_Data[37])
                {
                    t_37.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 4] == St_Data[38])
                {
                    t_38.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    t_35.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_36.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_37.BackgroundColor = Color.FromHex("#c5c6ff");
                    t_38.BackgroundColor = Color.FromHex("#c5c6ff");
                }
            }

            //Change color sellected shop
            _fr.BackgroundColor = Color.FromHex(st_HexColorGreen);


        }
        #endregion

        #region NetWork
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
                    ReceiveResponse();
                }
            });

        }

        private void ReceiveResponse()
        {
            var buffer = new byte[1024];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            if (int_step == 0)
            {
                TextReceive += Encoding.ASCII.GetString(data);
            }
            else
            {
                st_Stream = Encoding.ASCII.GetString(data);
                StreamProcessing(st_Stream);

            }

            Device.BeginInvokeOnMainThread(() =>
            {
                String b = TextReceive.Substring(TextReceive.Length - 6, 4);
                if (b == ".end")
                {
                    int_step++;
                    File.WriteAllText(fileData, TextReceive);

                    bt_Connect.IsVisible = false;
                    bt_Skip.IsVisible = false;
                    gr_page2.IsVisible = true;
                    unfixSet();
                    SetButtonShop(b_2, f_2, 0);
                }
            });
        }
        #endregion


        void StreamProcessing(String mess)
        {
            string a = "";
            try
            {
                a = mess.Substring(mess.Length - 6, 4);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            if (a == ".stream")
            {
                
            }
        }

        #region Button Shop
        private void b_5_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_5, f_5, 3);
        }

        private void b_4_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_4, f_4, 2);
        }

        private void b_3_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_3, f_3, 1);
        }

        private void b_2_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_2, f_2, 0);

        }

        private void b_6_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_6, f_6, 4);
        }

        private void b_7_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_7, f_7, 5);
        }

        private void b_8_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_8, f_8, 6);
        }

        private void b_9_Clicked(object sender, EventArgs e)
        {
            SetButtonShop(b_9, f_9, 7);
        }
        #endregion

        #region Button Score

        void SumScore()
        {
            int a = Convert.ToInt16(St_Result[int_CurrentShop, 2]);
            int b = Convert.ToInt16(St_Result[int_CurrentShop, 3]);
            int c = Convert.ToInt16(St_Result[int_CurrentShop, 4]);
            if (a*b*c >0)
            {
                bt_save.Text = "Save (Total " + (a+b+c).ToString() + ")";
            }
            else
            {
                bt_save.Text = "SAVE";
            }
        }
        void pressScoreC1(int stt)
        {
            t_27.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_28.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_29.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_30.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            int_sumScore[int_CurrentShop, 0] = int_score[stt];
            St_Result[int_CurrentShop,2] = int_score[stt].ToString();
            AllResult = "";
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 6; j++)
                {
                    AllResult += St_Result[i, j] + "-";
                }
                AllResult += "|";
            }
            SumScore();
        }

        void pressScoreC2(int stt)
        {
            t_31.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_32.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_33.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_34.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            int_sumScore[int_CurrentShop, 1] = int_score[stt];
            St_Result[int_CurrentShop, 3] = int_score[stt].ToString();
            AllResult = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    AllResult += St_Result[i, j] + "-";
                }
                AllResult += "|";
            }
            SumScore();
        }

        void pressScoreC3(int stt)
        {
            t_35.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_36.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_37.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            t_38.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            int_sumScore[int_CurrentShop, 2] = int_score[stt];
            St_Result[int_CurrentShop, 4] = int_score[stt].ToString();
            AllResult = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    AllResult += St_Result[i, j] + "-";
                }
                AllResult += "|";
            }
            SumScore();
        }

        // c1
        private void t_28_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(28); t_28.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_27_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(27); t_27.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_29_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(29); t_29.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_30_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(30); t_30.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }


        //c2 
        private void t_31_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(31); t_31.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_33_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(33); t_33.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_32_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(32); t_32.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_34_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(34); t_34.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        //c3
        private void t_35_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(35); t_35.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_36_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(36); t_36.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_37_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(37); t_37.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_38_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(38); t_38.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void bt_save_Clicked(object sender, EventArgs e)
        {
            int a = Convert.ToInt16(St_Result[int_CurrentShop, 2]);
            int b = Convert.ToInt16(St_Result[int_CurrentShop, 3]);
            int c = Convert.ToInt16(St_Result[int_CurrentShop, 4]);
            St_Result[int_CurrentShop, 1] = "1";
        }
        #endregion

    }
}