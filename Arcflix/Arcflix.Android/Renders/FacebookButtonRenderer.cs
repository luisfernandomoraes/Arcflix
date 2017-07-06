using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Telephony;
using Android.Widget;
using Arcflix.Controls;
using Arcflix.Droid.Renders;
using Arcflix.Models;
using Plugin.Toasts;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Interop;

[assembly: ExportRenderer(typeof(FacebookButton), typeof(FacebookButtonRenderer))]
namespace Arcflix.Droid.Renders
{
    public class FacebookButtonRenderer : ButtonRenderer
    {
        private LoginButton _loginButton;
        readonly List<string> _readPermissions = new List<string>
        {
            "public_profile"
        };

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;

            _loginButton = new LoginButton(Forms.Context);
            _loginButton.SetReadPermissions(_readPermissions);
            _loginButton.LoginBehavior = LoginBehavior.NativeWithFallback;

            //Implement FacebookCallback with LoginResult type to handle Callback's result
            var loginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {
                    /*
                        If login success, We can now retrieve our needed data and build our 
                        FacebookEventArgs parameters
                    */
                    FacebookButton facebookButton = (FacebookButton)e.NewElement;
                    FacebookEventArgs fbArgs = new FacebookEventArgs();
                    if (loginResult.AccessToken != null)
                    {
                        fbArgs.UserId = loginResult.AccessToken.UserId;
                        fbArgs.AccessToken = loginResult.AccessToken.Token;
                        var expires = loginResult.AccessToken.Expires;


                        var dateTokenExpiration = FromUnixTime(expires.Time);
                        fbArgs.TokenExpiration = dateTokenExpiration;
                    }
                    if (AccessToken.CurrentAccessToken != null)
                    {
                        GraphCallback graphCallBack = new GraphCallback();
                        graphCallBack.RequestCompleted += (s, evt) =>
                        {
                            var response = evt.Response;
                            if (response.Error != null)
                            {
                                System.Diagnostics.Debug.WriteLine(response.Error);
                                return;
                            }
                            try
                            {
                                _loginButton.Enabled = false;

                                var user = User.FromRawJsonFacebook2Object(response.RawResponse);
                                Arcflix.Services.DB.ArcflixDBContext.UserDataBase.SaveItem(user);
                                App.Current.Properties["IsLoggedIn"] = true;

                                App.Current.ShowMainPage();

                            }
                            catch (Exception ex)
                            {
                                App.ShowToast(ToastNotificationType.Error, "ScannPrice", "Ocorreu um erro ao efetuar login pelo Facebook, por favor tente mais tarde.", 3);
                                App.Current.ShowMainPage();
                            }
                        };

                        var request = new GraphRequest(AccessToken.CurrentAccessToken, "/me?fields=name,gender,link", null, HttpMethod.Get, graphCallBack, "v2.9");


                        try
                        {
                            request.ExecuteAsync();
                        }
                        catch (Exception exception)
                        {
                            System.Diagnostics.Debug.WriteLine(exception);
                            return;
                        }

                    }
                    /*
                        Pass the parameters into Login method in the FacebookButton 
                        object and handle it on Xamarin.Forms side
                    */
                    facebookButton.Login(facebookButton, fbArgs);
                },


                HandleCancel = () =>
                {
                    //Handle any cancel the user has perform
                    System.Diagnostics.Debug.WriteLine("User cancel de login operation");
                },
                HandleError = loginError =>
                {
                    //Handle any error happends here
                    System.Diagnostics.Debug.WriteLine("Operation throws an error: " + loginError?.Cause?.Message);
                }
            };
            LoginManager.Instance.RegisterCallback(MainActivity.CallbackManager, loginCallback);


            //Set the LoginButton as NativeControl
            SetNativeControl(_loginButton);
        }
       
        public DateTime FromUnixTime(long unixTimeMillis)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTimeMillis);
        }

        /// <summary>
        /// FacebookCallback<TResult> class which will handle any result the FacebookActivity returns.
        /// </summary>
        /// <typeparam name="TResult">The callback result's type you will handle</typeparam>
        public class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
        {
            public Action HandleCancel { get; set; }
            public Action<FacebookException> HandleError { get; set; }
            public Action<TResult> HandleSuccess { get; set; }

            public void OnCancel()
            {
                var c = HandleCancel;
                c?.Invoke();
            }

            public void OnError(FacebookException error)
            {
                var c = HandleError;
                c?.Invoke(error);
            }

            public void OnSuccess(Java.Lang.Object result)
            {
                var c = HandleSuccess;
                c?.Invoke(result.JavaCast<TResult>());
            }
        }
        class GraphCallback : Java.Lang.Object, GraphRequest.ICallback
        {
            // Event to pass the response when it's completed
            public event EventHandler<GraphResponseEventArgs> RequestCompleted = delegate { };

            public void OnCompleted(GraphResponse reponse)
            {
                this.RequestCompleted(this, new GraphResponseEventArgs(reponse));
            }
        }

        public class GraphResponseEventArgs : EventArgs
        {
            GraphResponse _response;
            public GraphResponseEventArgs(GraphResponse response)
            {
                _response = response;
            }

            public GraphResponse Response { get { return _response; } }
        }
    }
}