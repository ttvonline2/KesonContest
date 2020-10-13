using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KesonContest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        #region var
        string fileData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");
        string filettGK = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ttgk.txt");
        string fileResult = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "result.txt");
        string fileFont = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "font.txt");
        string fileState = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "state.txt");

        bool bol_connected = false;
        String[] St_Data = new String[50];
        String[,] St_Result = new String[8, 5];
        String DefaulResult = "0-0-0-0-0|0-0-0-0-0|0-0-0-0-0|0-0-0-0-0|0-0-0-0-0|0-0-0-0-0|0-0-0-0-0|0-0-0-0-0";
        string st_HexColorOrange = "#ffb400";
        readonly string st_HexColorGreen = "#30f000";
        string st_HexColorBlue = "#195e83";
        string st_HexColorSave0 = "#018fff";
        string st_HexcolorScore0 = "#c5c6ff";
        IPAddress address;
        Socket ClientSocket = new Socket
          (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        String TextReceive;
        private const int PORT = 197;
        int int_step = 0;
        int int_CurrentShop = 0;
        int[] int_score = new int[40];
        string[] st_code = new string[5];
        int[,] int_sumScore = new int[8, 4];

        double db_FontSize = 13; // Theme name font size
        double db_FontThemeName = 13, db_FontChallenge = 15, db_FontDetail = 9.5, db_FontScore = 24,
            db_FontOEBBHD = 18, db_FontName = 10, db_FontSave = 24, db_FontShop = 17, db_FontSum = 10;

        private static System.Timers.Timer TimerConnect;

        #endregion
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            SetupGUIdata();
        }

        private void SetupGUIdata()
        {
            if (File.Exists(fileFont))
            {
                string _a = File.ReadAllText(fileFont);
                if (_a != null)
                {
                    try
                    {
                        db_FontSize = Convert.ToInt16(_a);
                        lb_SetSize.FontSize = db_FontSize;
                        db_FontThemeName = db_FontSize*13 / 13;
                        db_FontChallenge = db_FontSize * 15 / 13;
                        db_FontDetail = db_FontSize * 9.5 /13;
                        db_FontScore = db_FontSize * 24 /13;
                        db_FontOEBBHD = db_FontSize * 18 /13;
                        db_FontName = db_FontSize * 10 /13;
                        db_FontSave = db_FontSize * 24 /13;
                        db_FontShop = db_FontSize * 17 / 13;
                        db_FontSum = db_FontSize * 10 / 13;
                    }
                    catch(Exception ee)
                    {
                        Console.WriteLine(ee);
                        //DependencyService.Get<Toast>().Show("Fail to read font size");
                    }
                }
            }

        }
          
        #region Page1

        void tryConnect()
        {
            TimerConnect = new System.Timers.Timer(3000);
            // Hook up the Elapsed event for the timer. 
            TimerConnect.Elapsed += TimeEvent;
            TimerConnect.AutoReset = true;
            TimerConnect.Enabled = true;
        }

        private void TimeEvent(Object source, ElapsedEventArgs e)
        {
            if (!ClientSocket.Connected)
            {
                ClientSocket.Close();
                Thread.Sleep(50);
                Console.WriteLine("Create new ClientSocket");
                Socket _clSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ClientSocket = _clSocket;
                ConnectToServer();
                RequestLoop();
            }
        }

        void setup_string_code()
        {

            string _ttgk = et_ttgk.Text;
            if (_ttgk == null)
            {
                try
                {
                    string tt = File.ReadAllText(filettGK);
                    string[] line = tt.Split(':');

                    address = IPAddress.Parse(line[0]);
                    _ttgk = line[1];
                }
                catch
                {
                    Console.WriteLine("Catch of setup_string_code");
                    File.WriteAllText(filettGK, _ttgk);
                    string tt = File.ReadAllText(filettGK);
                    string[] line = tt.Split(':');
                    address = IPAddress.Parse(line[0]);
                    _ttgk = line[1];
                    //DependencyService.Get<Toast>().Show("Used current address");
                }

            }
            else
            {
                try
                {
                    File.WriteAllText(filettGK, _ttgk);
                    string tt = File.ReadAllText(filettGK);
                    string[] line = tt.Split(':');
                    address = IPAddress.Parse(line[0]);
                    _ttgk = line[1];
                }
                catch
                {
                    Console.WriteLine("Catch of String Code");
                    // DependencyService.Get<Toast>().Show("The current address have problem");
                }

            }
            st_code[0] = "~~*cn" + _ttgk; // Thong bao cho SV da connected
            st_code[1] = "~~*da";// request data all.
            st_code[2] = "~~*re" + _ttgk; // Send data to server

        }
        
        private  void bt_Connect_Clicked(object sender, EventArgs e)
        {
            setup_string_code();
            File.WriteAllText(fileData, "");    // Delete old data
            File.WriteAllText(fileResult, DefaulResult);
            File.WriteAllText(fileState, "0");
            TextReceive = "";
            int_step = 5;
            tryConnect();
        }

        private void bt_Skip_Clicked(object sender, EventArgs e)
        {
            int_step = 0;
            setup_string_code();
            tryConnect();
            gr_page1.IsVisible = false;
            gr_page2.IsVisible = true;

            unfixSet();
            ScanOldResult();
            SetButtonShop(b_2, f_2, 0);
        }
        private void bt_ResetResult_Clicked(object sender, EventArgs e)
        {
            int_step = 0;
            setup_string_code();
            tryConnect();
            File.WriteAllText(fileResult, DefaulResult);
            File.WriteAllText(fileState, "0");
            gr_page1.IsVisible = false;
            gr_page2.IsVisible = true;

            unfixSet();
            ScanOldResult();
            SetButtonShop(b_2, f_2, 0);
        }


        void ConnectToServer()
        {

            if (!ClientSocket.Connected)
            {
                try
                {
                    bol_connected = true;
                    ClientSocket.Connect(address, PORT);
                    SendString(st_code[0]);
                    Console.WriteLine("Connect to server Successfully"); im_connect.Opacity = 1;
                    if (int_step >0)
                    {
                        SendString(st_code[1]);
                        int_step = 0;
                    }
                }
                catch
                {
                    ShowDisconnect();
                    Console.WriteLine("Catch of  Connect to server");
                }
            }

        }
        void add_avatar(int vt_shop)
        {
            string a = DependencyService.Get<getPathAndroid>().StringPathAndroid();
            string b = vt_shop.ToString() + ".JPG";
            string c = System.IO.Path.Combine(a, b);
            im_avatar.Source = c;
        }

        private void SetupFontSize(double _size)
        {
            lb_SetSize.FontSize = _size;
            File.WriteAllText(fileFont, _size.ToString());
            db_FontThemeName = db_FontSize * 13 / 13;
            db_FontChallenge = db_FontSize * 15 / 13;
            db_FontDetail = db_FontSize * 9.5 / 13;
            db_FontScore = db_FontSize * 24 / 13;
            db_FontOEBBHD = db_FontSize * 18 / 13;
            db_FontName = db_FontSize * 10 / 13;
            db_FontSave = db_FontSize * 24 / 13;
            db_FontShop = db_FontSize * 17 / 13;
            db_FontSum = db_FontSize * 10 / 13;
        }
        private void bt_SizeUp_Clicked(object sender, EventArgs e)
        {
            db_FontSize += 0.5;
            SetupFontSize(db_FontSize);

        }

        private void bt_SizeDown_Clicked(object sender, EventArgs e)
        {
            db_FontSize -= 0.5;
            SetupFontSize(db_FontSize);
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
            t_0.Text = St_Data[0]; t_0.FontSize = db_FontOEBBHD;
            t_1.Text = St_Data[1]; t_1.FontSize = db_FontOEBBHD;
            t_2.Text = St_Data[2]; t_2.FontSize = db_FontShop;
            t_3.Text = St_Data[3]; t_3.FontSize = db_FontShop;
            t_4.Text = St_Data[4]; t_4.FontSize = db_FontShop;
            t_5.Text = St_Data[5]; t_5.FontSize = db_FontShop;
            t_6.Text = St_Data[6]; t_6.FontSize = db_FontShop;
            t_7.Text = St_Data[7]; t_7.FontSize = db_FontShop;
            t_8.Text = St_Data[8]; t_8.FontSize = db_FontShop;
            t_9.Text = St_Data[9]; t_9.FontSize = db_FontShop;
            t_10To17.Text = St_Data[10]; t_10To17.FontSize = db_FontThemeName;

            t_18.Text = St_Data[18]; t_18.FontSize = db_FontChallenge;
            t_19.Text = St_Data[19]; t_19.FontSize = db_FontChallenge;
            t_20.Text = St_Data[20]; t_20.FontSize = db_FontChallenge;
            t_21.Text = St_Data[21]; t_21.FontSize = db_FontDetail;
            t_22.Text = St_Data[22]; t_22.FontSize = db_FontDetail;
            t_23.Text = St_Data[23]; t_23.FontSize = db_FontDetail;
            t_24.Text = St_Data[24]; t_24.FontSize = db_FontDetail;
            t_25.Text = St_Data[25]; t_25.FontSize = db_FontDetail;
            t_26.Text = St_Data[26]; t_26.FontSize = db_FontDetail;

            l_27.Text = St_Data[27];l_27.FontSize = db_FontScore;
            l_28.Text = St_Data[28];l_28.FontSize = db_FontScore;
            l_29.Text = St_Data[29];l_29.FontSize = db_FontScore;
            l_30.Text = St_Data[30];l_30.FontSize = db_FontScore;
            l_31.Text = St_Data[31];l_31.FontSize = db_FontScore;
            l_32.Text = St_Data[32];l_32.FontSize = db_FontScore;
            l_33.Text = St_Data[33];l_33.FontSize = db_FontScore;
            l_34.Text = St_Data[34];l_34.FontSize = db_FontScore;
            l_35.Text = St_Data[35];l_35.FontSize = db_FontScore;
            l_36.Text = St_Data[36];l_36.FontSize = db_FontScore;
            l_37.Text = St_Data[37];l_37.FontSize = db_FontScore;
            l_38.Text = St_Data[38];l_38.FontSize = db_FontScore;
            l_avatarName.Text = St_Data[39]; l_avatarName.FontSize = db_FontName;
        }
        #endregion

        #region Stream DATA

        private void bt_Theme2_Clicked(object sender, EventArgs e)
        {
            string a = St_Data[1];
            string b = "Not yet time for " + a +" contest";
            DependencyService.Get<Toast>().Show(b);

        }
        void ScanOldResult()
        {
            String AllResult = File.ReadAllText(fileResult);
            string[] li = AllResult.Split('|');
            for (int i = 0; i < li.Length; i++)
            {
                string[] n = li[i].Split('-');
                if (n[0] == "1")
                {
                    St_Result[i, 4] = (Convert.ToInt16(n[1]) + Convert.ToInt16(n[2]) + Convert.ToInt16(n[3])).ToString();
                }
            }
            string _State = File.ReadAllText(fileState);
            if(_State == "1")
            {
                gr_Theme2.IsVisible = false;
            }
        }
        void inputVar()
        {
            String AllResult = File.ReadAllText(fileResult);
            string[] li = AllResult.Split('|');
            for (int i = 0; i < li.Length; i++)
            {
                string[] n = li[i].Split('-');
                for (int j = 0; j < 4; j++)
                {
                    St_Result[i, j] = n[j];
                }
            }
        }

        void ColorButton()
        {
            if (St_Result[0, 0] == "0")
            {
                f_2.BackgroundColor = Color.FromHex(st_HexColorBlue); c_2.IsVisible = false;
            }
            else
            {
                f_2.BackgroundColor = Color.FromHex(st_HexColorOrange); c_2.IsVisible = true; l_2.Text = St_Result[0, 4]; l_2.FontSize = db_FontSum;
            }

            if (St_Result[1, 0] == "0")
            {
                f_3.BackgroundColor = Color.FromHex(st_HexColorBlue); c_3.IsVisible = false;
            }
            else
            {
                f_3.BackgroundColor = Color.FromHex(st_HexColorOrange); c_3.IsVisible = true; l_3.Text = St_Result[1, 4]; l_3.FontSize = db_FontSum;
            }

            if (St_Result[2, 0] == "0")
            {
                f_4.BackgroundColor = Color.FromHex(st_HexColorBlue); c_4.IsVisible = false;
            }
            else
            {
                f_4.BackgroundColor = Color.FromHex(st_HexColorOrange); c_4.IsVisible = true; l_4.Text = St_Result[2, 4]; l_4.FontSize = db_FontSum;
            }

            if (St_Result[3, 0] == "0")
            {
                f_5.BackgroundColor = Color.FromHex(st_HexColorBlue); c_5.IsVisible = false;
            }
            else
            {
                f_5.BackgroundColor = Color.FromHex(st_HexColorOrange); c_5.IsVisible = true; l_5.Text = St_Result[3, 4]; l_5.FontSize = db_FontSum;
            }

            if (St_Result[4, 0] == "0")
            {
                f_6.BackgroundColor = Color.FromHex(st_HexColorBlue); c_6.IsVisible = false;
            }
            else
            {
                f_6.BackgroundColor = Color.FromHex(st_HexColorOrange); c_6.IsVisible = true; l_6.Text = St_Result[4, 4]; l_6.FontSize = db_FontSum;
            }

            if (St_Result[5, 0] == "0")
            {
                f_7.BackgroundColor = Color.FromHex(st_HexColorBlue); c_7.IsVisible = false;
            }
            else
            {
                f_7.BackgroundColor = Color.FromHex(st_HexColorOrange); c_7.IsVisible = true; l_7.Text = St_Result[5, 4]; l_7.FontSize = db_FontSum;
            }

            if (St_Result[6, 0] == "0")
            {
                f_8.BackgroundColor = Color.FromHex(st_HexColorBlue); c_8.IsVisible = false;
            }
            else
            {
                f_8.BackgroundColor = Color.FromHex(st_HexColorOrange); c_8.IsVisible = true; l_8.Text = St_Result[6, 4]; l_8.FontSize = db_FontSum;
            }

            if (St_Result[7, 0] == "0")
            {
                f_9.BackgroundColor = Color.FromHex(st_HexColorBlue); c_9.IsVisible = false;
            }
            else
            {
                f_9.BackgroundColor = Color.FromHex(st_HexColorOrange); c_9.IsVisible = true; l_9.Text = St_Result[7, 4]; l_9.FontSize = db_FontSum;
            }

        }
        void SetButtonShop(Button _bt, Frame _fr, int stt)
        {

            int_CurrentShop = stt;
            inputVar();
            //Update other shop
            ColorButton();



            //Change Theme Name
            t_10To17.Text = St_Data[10 + stt]; t_10To17.FontSize = db_FontThemeName;
            l_avatarName.Text = St_Data[39 + stt];l_avatarName.FontSize = db_FontName;

            //Reset color Sorce

            f_27.BackgroundColor = Color.FromHex("#c5c6ff");
            f_28.BackgroundColor = Color.FromHex("#c5c6ff");
            f_29.BackgroundColor = Color.FromHex("#c5c6ff");
            f_30.BackgroundColor = Color.FromHex("#c5c6ff");

            f_31.BackgroundColor = Color.FromHex("#c5c6ff");
            f_32.BackgroundColor = Color.FromHex("#c5c6ff");
            f_33.BackgroundColor = Color.FromHex("#c5c6ff");
            f_34.BackgroundColor = Color.FromHex("#c5c6ff");

            f_35.BackgroundColor = Color.FromHex("#c5c6ff");
            f_36.BackgroundColor = Color.FromHex("#c5c6ff");
            f_37.BackgroundColor = Color.FromHex("#c5c6ff");
            f_38.BackgroundColor = Color.FromHex("#c5c6ff");

            // Reset Text at Save Button
            SumScore();
            f_save.BackgroundColor = Color.FromHex(st_HexColorSave0);

            //Change color score of shop
            if (St_Result[stt, 0] == "1")
            {
                _bt.IsVisible = true;
                f_save.BackgroundColor = Color.FromHex(st_HexColorOrange);

                //c1
                if (St_Result[stt, 1] == St_Data[27])
                {
                    f_27.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 1] == St_Data[28])
                {
                    f_28.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 1] == St_Data[29])
                {
                    f_29.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 1] == St_Data[30])
                {
                    f_30.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    f_27.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_28.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_29.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_30.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c2

                if (St_Result[stt, 2] == St_Data[31])
                {
                    f_31.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[32])
                {
                    f_32.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[33])
                {
                    f_33.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[34])
                {
                    f_34.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    f_31.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_32.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_33.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_34.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c3

                if (St_Result[stt, 3] == St_Data[35])
                {
                    f_35.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[36])
                {
                    f_36.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[37])
                {
                    f_37.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[38])
                {
                    f_38.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    f_35.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_36.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_37.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_38.BackgroundColor = Color.FromHex("#c5c6ff");
                }


            }
            else
            {
                _fr.BackgroundColor = Color.FromHex(st_HexColorGreen);
                //c1
                if (St_Result[stt, 1] == St_Data[27])
                {
                    f_27.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 1] == St_Data[28])
                {
                    f_28.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 1] == St_Data[29])
                {
                    f_29.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 1] == St_Data[30])
                {
                    f_30.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    f_27.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_28.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_29.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_30.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c2

                if (St_Result[stt, 2] == St_Data[31])
                {
                    f_31.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[32])
                {
                    f_32.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[33])
                {
                    f_33.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 2] == St_Data[34])
                {
                    f_34.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    f_31.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_32.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_33.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_34.BackgroundColor = Color.FromHex("#c5c6ff");
                }

                // c3

                if (St_Result[stt, 3] == St_Data[35])
                {
                    f_35.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[36])
                {
                    f_36.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[37])
                {
                    f_37.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else if (St_Result[stt, 3] == St_Data[38])
                {
                    f_38.BackgroundColor = Color.FromHex(st_HexColorOrange);
                }
                else
                {
                    f_35.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_36.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_37.BackgroundColor = Color.FromHex("#c5c6ff");
                    f_38.BackgroundColor = Color.FromHex("#c5c6ff");
                }
            }

            //Change color sellected shop
            _fr.BackgroundColor = Color.FromHex(st_HexColorGreen);

            add_avatar(stt + 1);

        }
        #endregion

        #region NetWork
        void SendString(string text)
        {
            if(ClientSocket.Connected)
            {
                Console.WriteLine("Successfully SendString");
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
            else
            {
                Console.WriteLine("Catch of SendString");
                ShowDisconnect();
            }


        }

        void ShowDisconnect()
        {
            bol_connected = false;
            im_connect.Opacity = 0.2;
        }
        async void RequestLoop()
        {
            await Task.Run(() =>
            {
                while (ClientSocket.Connected)
                {
                    try
                    {
                        ReceiveResponse();
                    }
                    catch
                    {
                        Console.WriteLine("Catch of RequestLoop");

                    }
                    
                }

            });
           // DependencyService.Get<Toast>().Show("Server disconnect!");
        }

        void ReceiveResponse()
        {
            var buffer = new byte[1024];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            TextReceive += Encoding.ASCII.GetString(data);

            Device.BeginInvokeOnMainThread(() =>
            {
                string b = "";
                try
                {
                    b = TextReceive.Substring(TextReceive.Length - 6, 4);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Catch of ReceiveResponse");
                }
                if (b == ".end")
                {
                    File.WriteAllText(fileData, TextReceive);
                    gr_page1.IsVisible = false;
                    gr_page2.IsVisible = true;
                    unfixSet();
                    ColorButton();
                    SetButtonShop(b_2, f_2, 0);
                    TextReceive = "";
                }
                else if (b == ".dat")
                {
                    String AllResult = File.ReadAllText(fileResult);
                    SendString(st_code[2] + AllResult);
                    TextReceive = "";
                }
                else if (b == "*not")
                {
                    DisplayAlert("You have a message", TextReceive.Substring(0, (TextReceive.Length - 6)), "Oke");
                    TextReceive = "";
                }
                else if (b == "*nex")
                {
                    gr_Theme2.IsVisible = false;
                    File.WriteAllText(fileState, "1");
                    TextReceive = "";
                }
            });
        }
        #endregion

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
        void SubmitData()
        {
            String AllResult = File.ReadAllText(fileResult);
            SendString(st_code[2] + AllResult);
        }
        void SumScore()
        {
            int a = Convert.ToInt16(St_Result[int_CurrentShop, 1]);
            int b = Convert.ToInt16(St_Result[int_CurrentShop, 2]);
            int c = Convert.ToInt16(St_Result[int_CurrentShop, 3]);
            if (a * b * c > 0)
            {
                l_save.Text = "SAVE (Total " + (a + b + c).ToString() + ")";
                l_save.FontSize = db_FontSave;
                if (St_Result[int_CurrentShop, 4] != (a + b + c).ToString())
                {
                    f_save.BackgroundColor = Color.FromHex(st_HexColorSave0);
                    HideCheckDone();
                }
            }
            else
            {
                l_save.Text = "SAVE";
                l_save.FontSize = db_FontSave;
            }

        }
        void pressScoreC1(int stt)
        {
            f_27.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_28.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_29.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_30.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            int_sumScore[int_CurrentShop, 0] = int_score[stt];
            St_Result[int_CurrentShop, 1] = int_score[stt].ToString();
            String AllResult = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 3)
                    {
                        AllResult += St_Result[i, j];
                    }
                    else
                    {
                        AllResult += St_Result[i, j] + "-";
                    }

                }
                if (i != 7)
                {
                    AllResult += "|";
                }
            }
            File.WriteAllText(fileResult, AllResult);
            SumScore();
            SubmitData();
        }

        void pressScoreC2(int stt)
        {
            f_31.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_32.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_33.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_34.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            int_sumScore[int_CurrentShop, 1] = int_score[stt];
            St_Result[int_CurrentShop, 2] = int_score[stt].ToString();
            String AllResult = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 3)
                    {
                        AllResult += St_Result[i, j];
                    }
                    else
                    {
                        AllResult += St_Result[i, j] + "-";
                    }

                }
                if (i != 7)
                {
                    AllResult += "|";
                }
            }
            File.WriteAllText(fileResult, AllResult);
            SumScore();
            SubmitData();
        }

        void pressScoreC3(int stt)
        {
            f_35.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_36.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_37.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            f_38.BackgroundColor = Color.FromHex(st_HexcolorScore0);
            int_sumScore[int_CurrentShop, 2] = int_score[stt];
            St_Result[int_CurrentShop, 3] = int_score[stt].ToString();
            String AllResult = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 3)
                    {
                        AllResult += St_Result[i, j];
                    }
                    else
                    {
                        AllResult += St_Result[i, j] + "-";
                    }

                }
                if (i != 7)
                {
                    AllResult += "|";
                }
            }
            File.WriteAllText(fileResult, AllResult);
            SumScore();
            SubmitData();
        }

        // c1
        private void t_28_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(28); f_28.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_27_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(27); f_27.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_29_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(29); f_29.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_30_Clicked(object sender, EventArgs e)
        {
            pressScoreC1(30); f_30.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }
        //c2 
        private void t_31_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(31); f_31.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_33_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(33); f_33.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }


        private void bt_Pass_Clicked_1(object sender, EventArgs e)
        {
            if (et_pass.Text == "1997")
            {
                gr_admin.IsVisible = false;
                gr_address.IsVisible = true;
                gr_setting.IsVisible = true;
            }
            else
            {
                DependencyService.Get<Toast>().Show("Incorrect password! Please press Continue");
            }
        }

        private void t_32_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(32); f_32.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_34_Clicked(object sender, EventArgs e)
        {
            pressScoreC2(34); f_34.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        //c3
        private void t_35_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(35); f_35.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_36_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(36); f_36.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_37_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(37); f_37.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private void t_38_Clicked(object sender, EventArgs e)
        {
            pressScoreC3(38); f_38.BackgroundColor = Color.FromHex(st_HexColorOrange);
        }

        private async void bt_save_Clicked(object sender, EventArgs e)
        {
            int a = Convert.ToInt16(St_Result[int_CurrentShop, 1]);
            int b = Convert.ToInt16(St_Result[int_CurrentShop, 2]);
            int c = Convert.ToInt16(St_Result[int_CurrentShop, 3]);

            if (a * b * c > 0)
            {
                St_Result[int_CurrentShop, 0] = "1";
                String AllResult = "";
                St_Result[int_CurrentShop, 4] = (a + b + c).ToString();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (j == 3)
                        {
                            AllResult += St_Result[i, j];
                        }
                        else
                        {
                            AllResult += St_Result[i, j] + "-";
                        }

                    }
                    if (i != 7)
                    {
                        AllResult += "|";
                    }
                }
                ShowCheckDone();
                File.WriteAllText(fileResult, AllResult);
                SendString(st_code[2] + AllResult);
                f_save.BackgroundColor = Color.FromHex(st_HexColorOrange);
                DependencyService.Get<Toast>().Show("Saved Successfully");
            }
            else
            {
                DependencyService.Get<Toast>().Show("Please mark before saving");
                if (St_Result[int_CurrentShop, 1] == "0")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        await f_c0.FadeTo(0, 200);
                        await f_c0.FadeTo(0.7, 200);
                    }
                }
                if (St_Result[int_CurrentShop, 2] == "0")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        await f_c1.FadeTo(0, 200);
                        await f_c1.FadeTo(0.7, 200);
                    }
                }

                if (St_Result[int_CurrentShop, 3] == "0")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        await f_c2.FadeTo(0, 200);
                        await f_c2.FadeTo(0.7, 200);
                    }
                }


            }

            void ShowCheckDone()
            {

                switch (int_CurrentShop)
                {
                    case 0:
                        l_2.Text = St_Result[int_CurrentShop, 4];l_2.FontSize = db_FontSum;
                        c_2.IsVisible = true;
                        
                        break;
                    case 1:
                        l_3.Text = St_Result[int_CurrentShop, 4]; l_3.FontSize = db_FontSum;
                        c_3.IsVisible = true;
                        break;
                    case 2:
                        l_4.Text = St_Result[int_CurrentShop, 4]; l_4.FontSize = db_FontSum;
                        c_4.IsVisible = true;
                        break;
                    case 3:
                        l_5.Text = St_Result[int_CurrentShop, 4]; l_5.FontSize = db_FontSum;
                        c_5.IsVisible = true;
                        break;

                    case 4:
                        l_6.Text = St_Result[int_CurrentShop, 4]; l_6.FontSize = db_FontSum;
                        c_6.IsVisible = true;
                        break;
                    case 5:
                        l_7.Text = St_Result[int_CurrentShop, 4]; l_7.FontSize = db_FontSum;
                        c_7.IsVisible = true;
                        break;
                    case 6:
                        l_8.Text = St_Result[int_CurrentShop, 4]; l_8.FontSize = db_FontSum;
                        c_8.IsVisible = true;
                        break;
                    case 7:
                        l_9.Text = St_Result[int_CurrentShop, 4]; l_9.FontSize = db_FontSum;
                        c_9.IsVisible = true;
                        break;
                }
                SubmitData();

            }
        }
        void HideCheckDone()
        {
            St_Result[int_CurrentShop, 0] = "0";
            String AllResult = "";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 3)
                    {
                        AllResult += St_Result[i, j];
                    }
                    else
                    {
                        AllResult += St_Result[i, j] + "-";
                    }

                }
                if (i != 7)
                {
                    AllResult += "|";
                }
            }
            File.WriteAllText(fileResult, AllResult);
            switch (int_CurrentShop)
            {
                case 0:
                    c_2.IsVisible = false;
                    break;
                case 1:
                    c_3.IsVisible = false;
                    break;
                case 2:
                    c_4.IsVisible = false;
                    break;
                case 3:
                    c_5.IsVisible = false;
                    break;

                case 4:
                    c_6.IsVisible = false;
                    break;
                case 5:
                    c_7.IsVisible = false;
                    break;
                case 6:
                    c_8.IsVisible = false;
                    break;
                case 7:
                    c_9.IsVisible = false;
                    break;
            }
        }
        #endregion



    }
}