using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Lobster-Regular.ttf", Alias = "Lobster")]
[assembly: ExportFont("Cooper Regular.ttf", Alias = "font1")]
[assembly: ExportFont("NotoSansKR-Medium.otf", Alias = "noto")]

namespace KesonContest
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new[] { "Shapes_Experimental", "Brush_Experimental" });
            InitializeComponent();
            MainPage =  new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
