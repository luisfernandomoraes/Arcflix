using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using ImageCircle.Forms.Plugin.Droid;
using Java.Security;
using Plugin.Toasts;
using Xamarin.Facebook;
using Signature = Android.Content.PM.Signature;

namespace Arcflix.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity MainActivityInstance;
        public static ICallbackManager CallbackManager;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            MainActivityInstance = this;

#if DEBUG //Show fb debug hash

            // Add code to print out the key hash
            try
            {
                PackageInfo info = PackageManager.GetPackageInfo("com.companyname.Arcflix", PackageInfoFlags.Signatures);
                foreach (Signature signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());
                    System.Diagnostics.Debug.WriteLine("KeyHash:", Base64.EncodeToString(md.Digest(), Base64.Default));
                }
            }
            catch (PackageManager.NameNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            catch (NoSuchAlgorithmException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.Droid.IconControls.Init();
            CallbackManager = CallbackManagerFactory.Create();
            ToastNotificatorImplementation.Init(this);
            ImageCircleRenderer.Init();

            LoadApplication(new App());
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ToString());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}