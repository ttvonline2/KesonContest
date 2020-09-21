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
        String alldatatest = "BBHD Themes|OE  Themes|MHS|PST|PRO|BLR|BLR|WTR|PST|PM|Cost saving 390,000 USD & shorting Lead time manufacturing by change design of Gemalink RMQC project.|Cost saving 233,000 USD shorting Lead time manufacturing by change erection method for MED BAPCO project.|Cost saving 30,000 USD and reduce time for Customs clearance by achieving AEO certification and set up internal procedures.|Ensuring quality and cost saving 782,000 USD applying for 9Cr Panel of CFB Boiler Sodegaura project.|Improve the productivity and quality of Coil products through Keson to clean surface of tube & visualization.|Improve the productivity and ensure zero site claim for MED BAPCO through Keson to clean surface of H-Beam. |Optimize the import & export process through Keson Container Dock & Jig handing stainless steel.|Improve the productivity and quality for erection operation of Samsung project  through Keson insert piping.|Challenge (30%)|Execution (30%)|Impact (40%)|(1) Theme is challenging and offers the value predominationg over other comertitors|(2) Find important and necessary factors for innovation?|(1) Progress of old method junking and problem solving in creative and proper way?|(2) Unhesitationg mobilizing manpower to complete the theme sucessfully?|(1) How much does contribute to increase/enhance company's competitivess|(2) Keson' result leads to effectively working method and make the continues effect?|24|26|28|30|24|26|28|30|34|36|38|40|";
        string fileData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.txt");
        string fileResult = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "result.txt");
        String[] St_Data = new String[39]; 
        public Page2()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void SetData()
        {
           
        }
        void inputVar()
        {
            string[] line = alldatatest.Split('|');
            for (int i = 0; i < 39; i ++)
            {
                St_Data[i] = line[i];
            }
        }
    }
}