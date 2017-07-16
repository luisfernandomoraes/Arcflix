using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcflix.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
            BindingContext = new MasterViewModel();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var navigationItem = e.SelectedItem as NavigationItem;
            navigationItem?.Command.Execute(navigationItem.PageType);
            MenuItensListView.SelectedItem = null;
        }
    }
}