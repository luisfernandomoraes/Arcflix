using System.Collections;
using System.Net.TMDb;
using Arcflix.ViewModels.Shows;
using Xamarin.Forms;

namespace Arcflix.Views.Shows
{
    public partial class ShowsPage : ContentPage
    {
        ShowsViewModel _viewModel;
        public SearchBar SearchBarShows => searchBarShows;

        public ShowsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ShowsViewModel(this);
            
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Show;
            if (item == null)
                return;

            await Navigation.PushAsync(new Shows.ShowDetailPage(item));

            // Manually deselected item
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Shows.Count == 0)
                _viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ItemsListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = this.ItemsListView.ItemsSource as IList;
            if (items != null && e.Item == _viewModel.Shows[items.Count - 1] && string.IsNullOrEmpty(_viewModel.Filter))
            {
                await _viewModel.LoadMore();
            }
        }
    }
}
