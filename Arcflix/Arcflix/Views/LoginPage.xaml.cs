﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcflix.Services;
using Arcflix.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public bool IsBusy;
        public LoginPage(ILoginManager ilm)
        {
            Title = "Login";
            InitializeComponent();
            BindingContext = new LoginViewModel(ilm);
            InitializeComponent();
            IsBusy = false;
            FacebookButton.Clicked += FacebookButton_Clicked;
        }

        private void FacebookButton_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VideoPlayer.Play();
        }
    }
}