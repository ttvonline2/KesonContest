using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KesonContest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void bt_Connect_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());


        }

        private async void bt_Skip_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());

        }
    }
}