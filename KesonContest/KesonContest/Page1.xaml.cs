using System;
using System.Collections.Generic;
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
        }

        private void bt_Skip_Clicked(object sender, EventArgs e)
        {
            var pageOne = new Page1();
            NavigationPage.SetHasNavigationBar(pageOne, false);
            NavigationPage mypage = new NavigationPage(pageOne);
        }
    }
}