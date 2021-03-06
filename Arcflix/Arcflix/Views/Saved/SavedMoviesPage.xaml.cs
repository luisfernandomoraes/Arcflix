﻿using System.Collections;
using System.Net.TMDb;
using Arcflix.Models;
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
            var item = args.SelectedItem as MovieModel;
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
        
    }
}
