using System;
using System.Security.Cryptography;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using ImageCircle.Forms.Plugin.Droid;
using Java.Security;
using Octane.Xam.VideoPlayer.Android;
using Plugin.Toasts;
using Xamarin.Facebook;
using Xamarin.Forms;
using Signature = Android.Content.PM.Signature;
using Xamarin.Facebook.Share.Widget;

namespace Arcflix.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity MainActivityInstance;
        public static ICallbackManager CallbackManager;
        public static Context AppMainContext;
        public static ShareDialog shareDialog;

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
            //Init FacebookSDK with the application context, then init an instance of CallbackManager
            FacebookSdk.SdkInitialize(this.ApplicationContext);

            CallbackManager = CallbackManagerFactory.Create();

            shareDialog = new Xamarin.Facebook.Share.Widget.ShareDialog(this);
            AppMainContext = ApplicationContext;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.Droid.IconControls.Init();
            CallbackManager = CallbackManagerFactory.Create();
            DependencyService.Register<ToastNotificatorImplementation>(); // Register your dependency
            ToastNotificatorImplementation.Init(this);
            ImageCircleRenderer.Init();
            FormsVideoPlayer.Init("CA185EF5ADE718EB678231E51058A935FE633A55");
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