using System;
using System.Linq;
using Android.App;
using Android.Runtime;
using Xamarin.Facebook;
using Xamarin.Facebook.Share;
using Xamarin.Facebook.Share.Widget;
using static Xamarin.Facebook.Share.Model.ShareLinkContent;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using Plugin.Toasts;
using Xamarin.Facebook.Internal;
using Arcflix.NativeCallsInterfaces;
using Arcflix.Droid.NativeCallsImpl;
using Facebook;

[assembly: Xamarin.Forms.Dependency(typeof(FacebookPostDroid))]
namespace Arcflix.Droid.NativeCallsImpl
{
    public class FacebookPostDroid : IFacebookPost
    {
        private string codetoPreview = "&feature=share";
        public async Task<bool> PostToWall(string urlImage, string message, string name, string link, string description)
        {
            //App.ShowToast(ToastNotificationType.Success, "ScannPrice", "Aguarde! esse recurso em breve estará disponível!", 3);
            //return true;

            var user = Services.DB.ArcflixDBContext.UserDataBase.GetItems().FirstOrDefault();

            using (var builder = new Builder())
            {
                Xamarin.Facebook.Share.Model.ShareLinkContent linkContent;

                //many links don't showing a previw , for this problem, use this trick (&feature=share)
                //using to shared recipes to faceboock 
                if (!String.IsNullOrEmpty(link) && link.ToLower().Contains("youtube"))
                {
                    var linkYoutube = String.Format(link + "{0}", codetoPreview);
                    linkContent = builder.SetContentTitle(message)
                   .SetImageUrl(Android.Net.Uri.Parse(linkYoutube))
                   .SetContentUrl(Android.Net.Uri.Parse(linkYoutube))
                   .JavaCast<Builder>()
                   .Build();
                }
                else
                {
                    linkContent = builder.SetContentTitle(message)
                       .SetContentDescription(description)
                       .SetImageUrl(Android.Net.Uri.Parse(urlImage))
                       .SetContentUrl(Android.Net.Uri.Parse("http://www.superalvorada.com.br/"))
                       .JavaCast<Builder>()
                       .Build();
                }

                using (var shareDialog = new ShareDialog(MainActivity.MainActivityInstance))
                {

                    try
                    {

                        // throw new System.Exception();
                        // Via dialog do facebook de compartilhamento.
                        if (!String.IsNullOrEmpty(link) && link.ToLower().Contains("youtube"))
                        {
                            shareDialog.Show(linkContent, ShareDialog.Mode.Automatic);
                        }
                        else
                        {
                            shareDialog.Show(linkContent, ShareDialog.Mode.Feed);
                        }

                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        return false;                        
                    }

                    return true;
                }
            }
        }

        private static void ShowAlert(string title, string msg, string buttonText = null)
        {
            try
            {
                using (var builder = new AlertDialog.Builder(MainActivity.AppMainContext)
                )
                {
                    builder.SetTitle(title)
                                  .SetMessage(msg)
                                  .SetPositiveButton(buttonText, (s2, e2) => { })
                                  .Show();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }

        }
    }

    internal class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
    {
        public Action HandleCancel { private get; set; }
        public Action<FacebookException> HandleError { private get; set; }
        public Action<TResult> HandleSuccess { private get; set; }

        public void OnCancel()
        {
            var c = HandleCancel;
            c?.Invoke();
        }

        public void OnError(FacebookException error)
        {
            HandleError?.Invoke(error);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var c = HandleSuccess;
            c?.Invoke(result.JavaCast<TResult>());
        }
    }
}