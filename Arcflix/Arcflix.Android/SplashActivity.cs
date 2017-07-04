using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Arcflix.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        static readonly string Tag = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(Tag, "SplashActivity.OnCreate");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            
            base.OnCreate(savedInstanceState);
        }
    }
}