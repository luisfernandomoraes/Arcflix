using System.Net.TMDb;
using Arcflix.Helpers;
using Arcflix.Models;
using Arcflix.Services;

using Xamarin.Forms;

namespace Arcflix.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the api service instance
        /// </summary>
        public IDataStore<Movie> MovieDataStore => DependencyService.Get<IDataStore<Movie>>();
        public IDataStore<Show> ShowDataStore => DependencyService.Get<IDataStore<Show>>();

        bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        private string _title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}

