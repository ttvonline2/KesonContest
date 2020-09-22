using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KesonContest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        String alldatatest = "OE\r\nThemes|BBHD Themes|MHS|PST|PRO|BLR|BLR|WTR|PST|PM|Cost saving 390,000 USD & shorting Lead time manufacturing by change design of Gemalink RMQC project.|Cost saving 233,000 USD shorting Lead time manufacturing by change erection method for MED BAPCO project.|Cost saving 30,000 USD and reduce time for Customs clearance by achieving AEO certification and set up internal procedures.|Ensuring quality and cost saving 782,000 USD applying for 9Cr Panel of CFB Boiler Sodegaura project.|Improve the productivity and quality of Coil products through Keson to clean surface of tube & visualization.|Improve the productivity and ensure zero site claim for MED BAPCO through Keson to clean surface of H-Beam. |Optimize the import & export process through Keson Container Dock & Jig handing stainless steel.|Improve the productivity and quality for erection operation of Samsung project  through Keson insert piping.|Challenge (30%)|Execution (30%)|Impact (40%)|(1) Theme is challenging and offers the value predominationg over other comertitors|(2) Find important and necessary factors for innovation?|(1) Progress of old method junking and problem solving in creative and proper way?|(2) Unhesitationg mobilizing manpower to complete the theme sucessfully?|(1) How much does contribute to increase/enhance company's competitivess|(2) Keson' result leads to effectively working method and make the continues effect?|24|26|28|30|24|26|28|30|34|36|38|40|";
        string fileData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");
        string fileResult = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "result.txt");
        String[] St_Data = new String[40]; 
        public Page2()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            SetData();
        }

        void SetData()
        {
            inputVar();
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
        void inputVar()
        {
            String AllData = File.ReadAllText(fileData);
            string[] line = AllData.Split('|');
            for (int i = 0; i < line.Length; i ++)
            {
                St_Data[i] = line[i];
                string a = St_Data[i];
            }
        }
    }
}