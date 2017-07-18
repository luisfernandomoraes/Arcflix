using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcflix.Controls;
using Arcflix.ViewModels.Saved;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Arcflix.Views.Saved
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedPage : PrettyTabbedPage
    {
        private SavedViewModel _vm;

        public SavedPage()
        {
            InitializeComponent();
            BindingContext = _vm = new SavedViewModel();
        }
    }
}