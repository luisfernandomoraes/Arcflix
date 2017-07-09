using System.Collections;
using System.Net.TMDb;
using Arcflix.ViewModels;
using Arcflix.ViewModels.Movies;
using Arcflix.ViewModels.Saved;
using Xamarin.Forms;

namespace Arcflix.Views.Saved
{
    public partial class SavedMoviesPage 
    {
        SavedMoviesViewModel _viewModel;
        public SearchBar SearchBarMovies => searchBarMovies;

        public SavedMoviesPage():base("")
        {
            InitializeComponent();

            BindingContext = _viewModel = new SavedMoviesViewModel(this);
            
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Movie;
            if (item == null)
                return;

            await Navigation.PushAsync(new SavedMovieDetailPage(item));

            // Manually deselected item
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Movies.Count == 0)
                _viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ItemsListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = this.ItemsListView.ItemsSource as IList;
            if (items != null && e.Item == _viewModel.Movies[items.Count - 1] && string.IsNullOrEmpty(_viewModel.Filter))
            {
                await _viewModel.LoadMore();
            }
        }
    }
}
