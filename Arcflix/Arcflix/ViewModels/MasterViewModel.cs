using System;
using System.Linq;
using System.Windows.Input;
using Arcflix.Controls;
using Arcflix.Models;
using Arcflix.Services.DB;
using Arcflix.Views;
using Arcflix.Views.Movies;
using Arcflix.Views.Saved;
using Arcflix.Views.Shows;
using Xamarin.Forms;

namespace Arcflix.ViewModels
{
    public class MasterViewModel
    {
        #region Fields

        public string ProfilePicture { get; set; }
        public string UserName { get; set; }

        private readonly User _currentUser;

        #endregion

        #region Commands

        public ICommand NavigationCommand => new Command(Navigation);

        private void Navigation(object obj)
        {
            var mdp = (Application.Current.MainPage as MasterDetailPage);
            var navPage = mdp.Detail as NavigationPage;

            // Hide the Master page
            mdp.IsPresented = false;

            switch (obj.ToString())
            {
                case "1":
                    mdp.Detail = new NavigationPage(new MoviesPage());
                    break;
                case "2":
                    mdp.Detail = new NavigationPage(new ShowsPage());
                    break;
                case "3":
                    mdp.Detail = new NavigationPage(new PrettyTabbedPage
                    {
                        Title = "Saved",
                        ShowTitles = false,
                        Children =
                    {
                        new SavedMoviesPage{Icon = "saved_movies",SelectedIcon = "saved_movies_selected"},
                        new SavedShowsPage{Icon = "shows_saved",SelectedIcon = "shows_saved_selected"},
                        }
                    });
                    break;
                case "4":
                    mdp.Detail = new NavigationPage(new SettingsPage());
                    break;
            }
        }

        #endregion

        #region Constructor

        public MasterViewModel()
        {
            _currentUser = ArcflixDBContext.UserDataBase.GetItems().FirstOrDefault();
            LoadUserProfile();
        }

        #endregion

        #region Methods

        private void LoadUserProfile()
        {
            ProfilePicture = _currentUser == null ? "profile_generic.png" : _currentUser.ProfilePictureURL;
            UserName = _currentUser == null ? "<Facebook login is required.>" : _currentUser.Name;

        }

        #endregion

    }
}