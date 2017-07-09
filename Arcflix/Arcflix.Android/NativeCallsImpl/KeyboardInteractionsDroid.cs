using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Arcflix.Droid.NativeCallsImpl;
using Arcflix.NativeCallsInterfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardInteractionsDroid))]

namespace Arcflix.Droid.NativeCallsImpl
{
    public class KeyboardInteractionsDroid : IKeyboardInteractions
    {
        public void HideKeyboard()
        {
            var inputMethodManager = Xamarin.Forms.Forms.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && Xamarin.Forms.Forms.Context is Activity)
            {
                var activity = Xamarin.Forms.Forms.Context as Activity;
                var token = activity.CurrentFocus == null ? null : activity.CurrentFocus.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, 0);
            }
        }
    }
}