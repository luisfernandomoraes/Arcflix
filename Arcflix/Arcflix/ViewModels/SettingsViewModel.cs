using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Arcflix.Helpers;
using Xamarin.Forms;

namespace Arcflix.ViewModels
{
    public class SettingsViewModel:BaseViewModel
    {
        #region Properties

        private string _apiKey;
        private string _language;

        public string ApiKey
        {
            get => _apiKey;
            set => SetProperty(ref _apiKey, value,nameof(ApiKey),()=> Settings.ApiKey = value);
        }

        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value,nameof(Language),()=>Settings.Language = value);
        }


        public string[] AvailableLanguages => new []{"en-US", "pt-BR", "de-DE" };

        #endregion

        #region Commands

        public ICommand SelectLanguageCommand => new Command(SelectLanguage);

        private async void SelectLanguage(object obj)
        {
            string cancelText = "Cancel";
            string okText = "OK";

            var result = await App.Current.MainPage.DisplayActionSheet("Language", cancelText, okText, AvailableLanguages);
            if (!string.IsNullOrEmpty(result) && cancelText != result && okText != result)
            {
                Language = result;
            }
        }

        #endregion

        #region Constructor

        public SettingsViewModel()
        {
            Title = "Settings";
            ApiKey = Settings.ApiKey;
            Language = Settings.Language;
        }

        #endregion
    }
}
