using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KesonContest.Droid
{
    public class AddImage : getPathAndroid
    {
        public void SetActivity(Activity activity)
        {

        }
        public string StringPathAndroid()
        {
            var sdCardPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
            return sdCardPath;
        }
    }
}