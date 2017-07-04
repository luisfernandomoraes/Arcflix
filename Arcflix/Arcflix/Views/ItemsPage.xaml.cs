using System;
using System.Collections;
using Arcflix.Models;
using Arcflix.ViewModels;

using Xamarin.Forms;

namespace Arcflix.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Items.Count == 0)
                _viewModel.LoadItemsCommand.Execute(null);
        }

        private async void ItemsListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = this.ItemsListView.ItemsSource as IList;
            if (items != null && e.Item == _viewModel.Items[items.Count - 1])
            {
                await _viewModel.LoadMore();
            }
        }
    }
}
