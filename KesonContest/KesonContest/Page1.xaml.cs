using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public Page1()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ReceiveData();
        }
        LinkServer ls = new LinkServer();

        private void bt_Connect_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Hellooo button");
            ls.Main();
        }

        private async void bt_Skip_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());

        }
        public async void ReceiveData()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (ls.CheckConnect() && (ls.ReceiveResponse() != null))
                    {
                        Console.WriteLine("TAo nhan dc r oi ne");
                    }
                    Thread.Sleep(1000);
                }
            });

        }
    }
}