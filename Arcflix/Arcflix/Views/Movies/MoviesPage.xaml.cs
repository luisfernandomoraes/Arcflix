using System.Collections;
using System.Net.TMDb;
using Arcflix.Models;
using Arcflix.ViewModels;
using Arcflix.ViewModels.Movies;
using Xamarin.Forms;

namespace Arcflix.Views.Movies
{
    public partial class MoviesPage : ContentPage
    {
        MoviesViewModel _viewModel;
        public SearchBar SearchBarMovies => searchBarMovies;
        public ListView MoviesListView => ItemsListView;
        public MoviesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MoviesViewModel(this);

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as MovieModel;
            if (item == null)
                return;

            await Navigation.PushAsync(new MovieDetailPage(item));

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

            var items = ItemsListView.ItemsSource as IList;
            if (items != null && e.Item == _viewModel.Movies[items.Count - 1])
            {
                if (_viewModel.IsVisibleSearchBar)
                    await _viewModel.LoadMoreSearchResults();
                else await _viewModel.LoadMore();
            }
        }
    }
}
