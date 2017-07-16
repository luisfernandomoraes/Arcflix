using System;
using System.Collections.ObjectModel;
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
    public class MasterViewModel:BaseViewModel
    {
        #region Fields

        public string ProfilePicture { get; set; }
        public string UserName { get; set; }

        private readonly User _currentUser;
        private ObservableCollection<NavigationItem> _navigationMenuItens;

        #endregion

        #region Properties

        public ObservableCollection<NavigationItem> NavigationMenuItens
        {
            get => _navigationMenuItens;
            set => SetProperty(ref _navigationMenuItens, value);
        }

        #endregion
        #region Commands

        public ICommand NavigationCommand => new Command(Navigation);

        private void Navigation(object obj)
        {
            var mdp = (Application.Current.MainPage as MasterDetailPage);

            var navigationPageType = Type.GetType(obj.ToString());

            var page = (Page)Activator.CreateInstance(navigationPageType);
            // Hide the Master page
            if (mdp != null)
            {
                mdp.IsPresented = false;

                mdp.Detail = new NavigationPage(page);
            }
        }

        #endregion

        #region Constructor

        public MasterViewModel()
        {
            _currentUser = ArcflixDBContext.UserDataBase.GetItems().FirstOrDefault();
            LoadNavigationsMenuItens();
            LoadUserProfile();
        }



        #endregion

        #region Methods
        private void LoadNavigationsMenuItens()
        {
            _navigationMenuItens = _navigationMenuItens ?? new ObservableCollection<NavigationItem>()
            {
                new NavigationItem
                {
                    Icon = "fa-film",
                    Text = "Upcoming Movies",
                    Command = NavigationCommand,
                    PageType = typeof(MoviesPage).ToString()
                    
                },
                new NavigationItem
                {
                    Icon = "fa-television",
                    Text = "Popular TV Shows",
                    Command = NavigationCommand,
                    PageType = typeof(ShowsPage).ToString()

                },new NavigationItem
                {
                    Icon = "fa-bookmark-o",
                    Text = "Saved",
                    Command = NavigationCommand,
                    PageType = typeof(SavedPage).ToString()

                },
                new NavigationItem
                {
                    ShowSeparatorLine = true,
                    Icon = "fa-cog",
                    Text = "Configurations",
                    Command = NavigationCommand,
                    PageType = typeof(SettingsPage).ToString()
                }
            };

        }
        private void LoadUserProfile()
        {
            ProfilePicture = _currentUser == null ? "profile_generic.png" : _currentUser.ProfilePictureURL;
            UserName = _currentUser == null ? "<Facebook login is required.>" : _currentUser.Name;

        }

        #endregion

    }
}