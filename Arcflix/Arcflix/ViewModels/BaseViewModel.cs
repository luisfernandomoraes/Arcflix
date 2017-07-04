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
        /// Get the azure service instance
        /// </summary>
        public IDataStore<Movie> DataStore => DependencyService.Get<IDataStore<Movie>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }
}

