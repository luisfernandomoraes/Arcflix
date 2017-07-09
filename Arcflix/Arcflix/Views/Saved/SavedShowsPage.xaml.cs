using System.Collections;
using System.Net.TMDb;
using Arcflix.Controls;
using Arcflix.ViewModels.Saved;
using Arcflix.ViewModels.Shows;
using Xamarin.Forms;

namespace Arcflix.Views.Saved
{
    public partial class SavedShowsPage : TabPage
    {
        SavedShowsViewModel _viewModel;
        public SearchBar SearchBarShows => searchBarShows;

        public SavedShowsPage():base("")
        {
            InitializeComponent();

            BindingContext = _viewModel = new SavedShowsViewModel(this);
            
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Show;
            if (item == null)
                return;

            await Navigation.PushAsync(new SavedShowDetailPage(item));

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
